namespace UzTube.Application.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string folderName, string fileName, Stream fileStream, string contentType, long fileSize);

    Task StreamFileAsync(string folderName, string fileName, Stream outputStream);

    Task<bool> FileExistsAsync(string bucketName, string objectName);

    Task<bool> RemoveFileAsync(string bucketName, string objectName);

    Task<bool> BucketExistsAsync(string bucketName);

    Task CreateBucketAsync(string bucketName);
}