using UzTube.Services;

namespace UzTube.Interfaces;

public interface IFileUploadService
{
    Task<Result<string>> UploadThumbnail(IFormFile file);
    Task<Result<string>> UploadVideo(IFormFile file);
}