using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class PostService(
    DatabaseContext context,
    IClaimService claimService,
    IFileStorageService fileStorageService
) : IPostService
{
    private const string VideoFolder = "videos";
    private const long MaxVideoSize = 10L * 1024 * 1024 * 1024; // 10GB

    private static readonly Dictionary<string, string> ContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Video
        [".mp4"] = "video/mp4",
        [".webm"] = "video/webm",
        [".mkv"] = "video/x-matroska",
        [".avi"] = "video/x-msvideo",
        [".mov"] = "video/quicktime"
    };

    private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".webm", ".mkv", ".avi", ".mov"
    };

    public async Task<UploadVideoFileResponseModel> UploadVideoFileAsync(IFormFile file)
    {
        ValidateVideoFile(file);

        string? extension = Path.GetExtension(file.FileName).ToLower();
        string? fileName = $"{Guid.NewGuid()}{extension}";

        await using Stream? stream = file.OpenReadStream();

        await fileStorageService.UploadFileAsync(
            VideoFolder,
            fileName,
            stream,
            GetContentType(fileName),
            file.Length
        );

        return new UploadVideoFileResponseModel { FileUrl = fileName };
    }

    public async Task StreamVideoFileAsync(string folderName, string fileName, HttpResponse response)
    {
        ValidateStreamRequest(folderName, fileName);

        response.ContentType = GetContentType(fileName);
        response.Headers.Append("Accept-Ranges", "bytes");
        response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileName}\"");

        await fileStorageService.StreamFileAsync(folderName, fileName, response.Body);
    }

    public async Task<CreatePostResponseModel> CreatePostAsync(CreatePostModel request)
    {
        Guid userId = claimService.GetUserId();

        Post newPost = new Post
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            ThumbnailUrl = request.ThumbnailUrl,
            VideoUrl = request.VideoUrl,
            Duration = "00:00:00" // TODO: Video file ni vaqtini hisoblididan qilish
        };

        await context.Posts.AddAsync(newPost);
        await context.SaveChangesAsync();

        return new CreatePostResponseModel { Id = newPost.Id };
    }

    public async Task<PostResponseModel> GetPostAsync(Guid id)
    {
        PostResponseModel? post = await context.Posts
            .Where(p => p.Id == id)
            .Select(p => new PostResponseModel
            {
                Id = id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.ThumbnailUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .FirstOrDefaultAsync();

        return post ?? throw new NotFoundException("Post not found");
    }

    public async Task<PaginatedList<PostResponseModel>> GetPostsAsync(PageOption option)
    {
        IQueryable<Post> query = context.Posts;

        if (!string.IsNullOrEmpty(option.Search))
            query = query.Where(p => p.Title.Contains(option.Search.Trim(), StringComparison.OrdinalIgnoreCase));

        List<PostResponseModel> posts = await query
            .Where(p => !p.IsPrivate)
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.ThumbnailUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        if (posts.Count == 0)
            throw new NotFoundException("Posts not found");

        int postsCount = await context.Posts.CountAsync();

        return PaginatedList<PostResponseModel>.Create(posts, postsCount, option.PageNumber, option.PageSize);
    }

    public async Task<PostResponseModel> SearchPostAsync(string query)
    {
        if (!string.IsNullOrEmpty(query))
            query = query.Trim();

        PostResponseModel? post = await context.Posts
            .Where(p => p.Title.Contains(query) && !p.IsPrivate)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.ThumbnailUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .FirstOrDefaultAsync();

        return post ?? throw new NotFoundException("Post not found");
    }

    public async Task<List<PostResponseModel>> GetUserPostsAsync(Guid userId, PageOption option)
    {
        IQueryable<Post> query = context.Posts.AsQueryable();

        List<PostResponseModel> posts = await query
            .Where(p => p.UserId == userId && !p.IsPrivate)
            .Skip(option.PageSize * (option.PageNumber - 1))
            .Take(option.PageSize)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.ThumbnailUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        return posts.Count == 0 ? throw new NotFoundException("Post not found") : posts;
    }

    public async Task<UpdatePostResponseModel> UpdatePostAsync(Guid id, UpdatePostRequest request)
    {
        Post post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new NotFoundException("Post not found");

        post.Title = request.Title;
        post.Description = request.Description;
        post.ThumbnailUrl = request.ThumbnailUrl;
        post.IsPrivate = request.IsPrivate;

        context.Posts.Update(post);
        await context.SaveChangesAsync();

        return new UpdatePostResponseModel { Id = post.Id };
    }

    public async Task<DeletePostResponseModel> DeletePostAsync(Guid id)
    {
        // TODO: When delete post need copy to HistoryTable
        Post post = await context.Posts.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("Post not found");

        post.IsDeleted = true;
        post.DeletedAt = DateTime.Now;

        await context.SaveChangesAsync();

        return new DeletePostResponseModel("Success");
    }

    public async Task<RestorePostResponseModel> RestorePostAsync(Guid userId)
    {
        Post post = await context.Posts.FirstOrDefaultAsync(u => u.Id == userId)
                    ?? throw new NotFoundException("Post not found");

        post.IsDeleted = false;
        post.DeletedAt = null;

        await context.SaveChangesAsync();

        return new RestorePostResponseModel("Success");
    }

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

    private static void ValidateStreamRequest(string folderName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(folderName))
            throw new BadRequestException("Folder name is required");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new BadRequestException("File name is required");
    }

    private static string GetContentType(string fileName)
    {
        string? ext = Path.GetExtension(fileName);

        return ContentTypes.GetValueOrDefault(ext, "application/octet-stream");
    }
}