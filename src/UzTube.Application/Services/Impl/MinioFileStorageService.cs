using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using UzTube.Application.Common.Minio;
using UzTube.Application.Exceptions;
using UzTube.Core.Common;

namespace UzTube.Application.Services.Impl;

public class MinioFileStorageService(
    IMinioClient minioClient,
    IOptions<MinioSettings> minioSettings
    ) : IFileStorageService
{
    private readonly MinioSettings _minioSettings = minioSettings.Value;

    private static readonly string AvatarFolder = SystemFolderNames.Avatar;
    private static readonly string VideoFolder = SystemFolderNames.Video;
    private static readonly string PreviewFolder = SystemFolderNames.Preview;

    private static readonly long MaxAvatarSize = SystemFileSizeLimit.Upload.Avatar;
    private static readonly long MaxVideoSize = SystemFileSizeLimit.Upload.Video;
    private static readonly long MaxPreviewSize = SystemFileSizeLimit.Upload.Preview;

    private static readonly Dictionary<string, string> ContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        // ======================
        // VIDEO (Native support)
        // ======================
        [".mp4"] = "video/mp4",    // H.264 + AAC (ENG STABIL)
        [".m4v"] = "video/mp4",
        [".mp4v"] = "video/mp4",

        [".webm"] = "video/webm",   // VP8 / VP9 (Chrome, Firefox, Edge)

        // ======================
        // THUMBNAIL & AVATAR (Native)
        // ======================
        [".jpg"] = "image/jpeg",   // ENG STABIL, ENG TEZ
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",    // Transparency support
        [".webp"] = "image/webp"    // ENG YAXSHI SIFAT + KICHIK HAJM
    };

    private static readonly HashSet<string> AllowedAvatarExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".m4v", ".mp4v", ".webm"
    };

    private static readonly HashSet<string> AllowedPreviewExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    public async Task<string> UploadAvatarFileAsync(IFormFile file)
    {
        ValidateAvatarFile(file);

        string? extension = GetExtension(file.FileName);
        string? folderName = AvatarFolder;
        string? fileName = $"{Guid.NewGuid()}{extension}";
        long fileLength = file.Length;
        string? contentType = file.ContentType;

        await using Stream? stream = file.OpenReadStream();

        string previewFileUrl = await UploadFileAsync(folderName, fileName, stream, contentType, fileLength);

        return previewFileUrl;
    }

    public async Task<string> UploadVideoFileAsync(IFormFile file)
    {
        ValidateVideoFile(file);

        string? extension = GetExtension(file.FileName);
        string? folderName = VideoFolder;
        string? fileName = $"{Guid.NewGuid()}{extension}";
        long fileLength = file.Length;
        string? contentType = file.ContentType;

        await using Stream? stream = file.OpenReadStream();

        string? videoFileUrl = await UploadFileAsync(folderName, fileName, stream, contentType, fileLength);

        return videoFileUrl;
    }

    public async Task<string> UploadPreviewFileAsync(IFormFile file)
    {
        ValidatePreviewFile(file);

        string? extension = GetExtension(file.FileName).ToLower();
        string? folderName = PreviewFolder;
        string? fileName = $"{Guid.NewGuid()}{extension}";
        long fileLength = file.Length;
        string? contentType = file.ContentType;

        await using Stream? stream = file.OpenReadStream();

        string previewFileUrl = await UploadFileAsync(folderName, fileName, stream, contentType, fileLength);

        return previewFileUrl;
    }

    public async Task<string> UploadFileAsync(
        string folderName,
        string fileName,
        Stream fileStream,
        string contentType,
        long fileSize = -1)
    {
        folderName = folderName.ToLower();
        fileName = fileName.ToLower();

        await EnsureFolderExistsAsync(folderName);

        PutObjectArgs? putObjectArgs = new PutObjectArgs()
            .WithBucket(folderName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(GetFileSize(fileSize, fileStream))
            .WithContentType(contentType);

        await minioClient.PutObjectAsync(putObjectArgs);

        return $"http://localhost:{_minioSettings.Port}/{folderName}/{fileName}";
    }

    public async Task StreamFileAsync(string folderName, string fileName, Stream outputStream)
    {
        folderName = folderName.ToLower();
        fileName = fileName.ToLower();

        await EnsureFileExistsAsync(folderName, fileName);

        TaskCompletionSource<bool>? tcs = new();
        Exception? exception = null;

        GetObjectArgs? getObjectArgs = new GetObjectArgs()
            .WithBucket(folderName)
            .WithObject(fileName)
            .WithCallbackStream(cs =>
            {
                try
                {
                    cs.CopyTo(outputStream);
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    tcs.SetResult(false);
                }
            });

        await minioClient.GetObjectAsync(getObjectArgs);
        await tcs.Task;

        if (exception is not null)
            throw exception;
    }

    public async Task DeleteFileAsync(string folderName, string fileName)
    {
        folderName = folderName.ToLower();
        fileName = fileName.ToLower();

        await EnsureFileExistsAsync(folderName, fileName);

        RemoveObjectArgs? removeArgs = new RemoveObjectArgs()
            .WithBucket(folderName)
            .WithObject(fileName);

        await minioClient.RemoveObjectAsync(removeArgs);
    }


    private async Task EnsureFolderExistsAsync(string folderName)
    {
        bool exists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(folderName));

        if (!exists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(folderName));
        }
    }

    private async Task EnsureFileExistsAsync(string folderName, string fileName)
    {
        bool bucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(folderName));

        if (!bucketExists)
            throw new NotFoundException($"Folder '{folderName}' not found");

        try
        {
            await minioClient.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(folderName)
                    .WithObject(fileName));
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            throw new NotFoundException($"File '{fileName}' not found");
        }
    }

    private static long GetFileSize(long fileSize, Stream fileStream)
        => fileSize > 0 ? fileSize : (fileStream.CanSeek ? fileStream.Length : -1);

    private static void ValidateVideoFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            throw new BadRequestException("Video file is required");

        string? extension = Path.GetExtension(file.FileName);

        if (string.IsNullOrEmpty(extension))
            throw new BadRequestException("File must have an extension");

        if (!AllowedVideoExtensions.Contains(extension))
            throw new BadRequestException($"Invalid format. Allowed: {string.Join(", ", AllowedVideoExtensions)}");

        if (file.Length > MaxVideoSize)
            throw new BadRequestException($"File size exceeds {MaxVideoSize / (1024 * 1024 * 1024)}GB limit");
    }

    private static void ValidatePreviewFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            throw new BadRequestException("Preview file is required");

        string? extension = Path.GetExtension(file.FileName);

        if (string.IsNullOrEmpty(extension))
            throw new BadRequestException("File must have an extension");

        if (!AllowedPreviewExtensions.Contains(extension))
            throw new BadRequestException($"Invalid format: {extension}. Allowed: {string.Join(", ", AllowedPreviewExtensions)}");

        if (file.Length > MaxPreviewSize)
            throw new BadRequestException($"File size exceeds {MaxPreviewSize / (1024 * 1024)}MB limit");
    }

    private static void ValidateAvatarFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            throw new BadRequestException("Avatar file is required");

        string? extension = GetExtension(file.FileName);

        if (string.IsNullOrEmpty(extension))
            throw new BadRequestException("File must have an extension");

        if (!AllowedAvatarExtensions.Contains(extension))
            throw new BadRequestException($"Invalid format: {extension}. Allowed: {string.Join(", ", AllowedAvatarExtensions)}");

        if (file.Length > MaxAvatarSize)
            throw new BadRequestException($"File size exceeds {MaxAvatarSize / (1024 * 1024)}MB limit");
    }

    private static string GetContentType(string extension) =>
        ContentTypes.GetValueOrDefault(extension, "application/octet-stream");

    private static string GetExtension(string extension) =>
        Path.GetExtension(extension).ToLower();
}