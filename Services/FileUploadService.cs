using Microsoft.Extensions.Options;
using UzTube.Interfaces;
using UzTube.Models.DTO;

namespace UzTube.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;
    private readonly IOptions<FileStorageOptions> _fileOptions;

    public FileUploadService(
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment env,
        IOptions<FileStorageOptions> fileOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
        _fileOptions = fileOptions;
    }

    public async Task<Result<string>> UploadThumbnail(IFormFile file)
    {
        HttpRequest? request = _httpContextAccessor.HttpContext?.Request;

        string[] allowedThumbnailExtensions = { ".jpg", ".png" };
        string thumbnailsFolder = Path.Combine(_env.WebRootPath, _fileOptions.Value.BaseUrl, _fileOptions.Value.Thumbnails);
        Directory.CreateDirectory(thumbnailsFolder);

        if (file == null)
            return new Result<string>
            {
                Succeed = false,
                Message = "Thumbnail is required",
                StatusCode = 400
            };

        string thumbnailExt = Path.GetExtension(file.FileName).ToLower();

        if (!allowedThumbnailExtensions.Contains(thumbnailExt))
            return new Result<string>
            {
                Succeed = false,
                Message = $"Thumbnail must be one of: {string.Join(", ", allowedThumbnailExtensions)}",
                StatusCode = 400
            };

        string thumbnailFileName = $"{Guid.NewGuid()}{thumbnailExt}";
        string thumbnailPath = Path.Combine(thumbnailsFolder, thumbnailFileName);

        using (FileStream stream = new FileStream(thumbnailPath, FileMode.Create))
            await file.CopyToAsync(stream);

        string thumbnailUrl = $@"{request?.Scheme}://{request?.Host}/{_fileOptions.Value.BaseUrl}/{_fileOptions.Value.Thumbnails}/{thumbnailFileName}";

        return new Result<string>
        {
            Succeed = true,
            StatusCode = 201,
            Data = thumbnailUrl
        };
    }

    public async Task<Result<string>> UploadVideo(IFormFile file)
    {
        HttpRequest? request = _httpContextAccessor.HttpContext?.Request;

        string[] allowedVideoExtensions = { ".mp4" };
        string videosFolder = Path.Combine(_env.WebRootPath, _fileOptions.Value.BaseUrl, _fileOptions.Value.Videos);
        Directory.CreateDirectory(videosFolder);

        if (file == null)
            return new Result<string>
            {
                Succeed = false,
                Message = "Video is required",
                StatusCode = 400
            };

        string videoExt = Path.GetExtension(file.FileName).ToLower();

        if (!allowedVideoExtensions.Contains(videoExt))
            return new Result<string>
            {
                Succeed = false,
                Message = $"Video must be one of: {string.Join(", ", allowedVideoExtensions)}",
                StatusCode = 400
            };

        string videoFileName = $"{Guid.NewGuid()}{videoExt}";
        string videoPath = Path.Combine(videosFolder, videoFileName);

        using (FileStream stream = new FileStream(videoPath, FileMode.Create))
            await file.CopyToAsync(stream);

        string videoUrl = $@"{request?.Scheme}://{request?.Host}/{_fileOptions.Value.BaseUrl}/{_fileOptions.Value.Videos}/{videoFileName}";

        return new Result<string>
        {
            Succeed = true,
            StatusCode = 201,
            Data = videoUrl
        };
    }
}
