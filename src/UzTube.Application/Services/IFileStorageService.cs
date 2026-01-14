using Microsoft.AspNetCore.Http;

namespace UzTube.Application.Services;

public interface IFileStorageService
{
    Task<string> UploadAvatarFileAsync(IFormFile file);

    Task<string> UploadVideoFileAsync(IFormFile file);

    Task<string> UploadPreviewFileAsync(IFormFile file);

    Task<string> UploadFileAsync(string folderName, string fileName, Stream fileStream, string contentType, long fileSize);

    Task StreamFileAsync(string folderName, string fileName, Stream outputStream);

    Task DeleteFileAsync(string folderName, string fileName);
}