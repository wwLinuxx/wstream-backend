using Minio;
using Minio.DataModel.Args;

namespace UzTube.Application.Services.Impl;

public class MinioFileStorageService(
    IMinioClient minioClient
) : IFileStorageService
{
    public async Task<string> UploadFileAsync(string folderName, string fileName, Stream fileStream, string contentType, long fileSize = -1)
    {
        string bucketName = folderName.ToLower();

        bool found = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName)
        );

        if (!found)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName)
            );
        }

        long size = fileSize > 0 ? fileSize : (fileStream.CanSeek ? fileStream.Length : -1);

        PutObjectArgs? putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName.ToLower())
            .WithStreamData(fileStream)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await minioClient.PutObjectAsync(putObjectArgs);

        return fileName;
    }

    public async Task StreamFileAsync(string folderName, string fileName, Stream outputStream)
    {
        TaskCompletionSource<bool>? tcs = new TaskCompletionSource<bool>();
        Exception? exception = null;

        GetObjectArgs? getObjectArgs = new GetObjectArgs()
            .WithBucket(folderName.ToLower())
            .WithObject(fileName.ToLower())
            .WithCallbackStream(cs =>
            {
                try
                {
                    byte[] buffer = new byte[81920]; // 80KB buffer
                    int bytesRead;
                    while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
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

        if (exception != null)
            throw exception;
    }

    public async Task<bool> FileExistsAsync(string bucketName, string objectName)
    {
        // StatObjectAsync fayl haqida ma'lumotni oladi, agar mavjud bo'lmasa xato tashlaydi
        await minioClient.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
        ).ConfigureAwait(false);

        return true; // Fayl mavjud
    }

    public async Task<bool> RemoveFileAsync(string bucketName, string objectName)
    {
        await minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
        ).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        return await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName)
        ).ConfigureAwait(false);
    }

    public async Task CreateBucketAsync(string bucketName)
    {
        bool found = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName)
        ).ConfigureAwait(false);

        if (!found)
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName)
            ).ConfigureAwait(false);
    }
}