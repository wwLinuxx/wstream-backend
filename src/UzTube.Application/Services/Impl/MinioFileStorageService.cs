using Minio;
using Minio.DataModel.Args;
using UzTube.Application.Exceptions;

namespace UzTube.Application.Services.Impl;

public class MinioFileStorageService(IMinioClient minioClient) : IFileStorageService
{
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

        return fileName;
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
}