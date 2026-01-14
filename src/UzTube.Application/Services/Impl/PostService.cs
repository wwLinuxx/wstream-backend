using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Core.Common;
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
    public async Task StreamVideoFileAsync(string fileName, HttpResponse response)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new BadRequestException("File name is required");

        string folderName = SystemFolderNames.Video;

        //response.ContentType = GetContentType(fileName);
        response.Headers.Append("Accept-Ranges", "bytes");
        response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileName}\"");

        await fileStorageService.StreamFileAsync(folderName, fileName, response.Body);
    }

    public async Task<CreatePostResponseModel> CreatePostAsync(CreatePostRequest request)
    {
        Guid userId = claimService.GetUserId();

        string? videoFileUrl = await fileStorageService.UploadVideoFileAsync(request.VideoFile);
        string? previewFileUrl = await fileStorageService.UploadPreviewFileAsync(request.PreviewFile);

        string? videoDuration = "24:12:60"; // TODO: Video ni vaqtini hisoblash

        Post newPost = new Post
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            VideoUrl = videoFileUrl,
            PreviewUrl = previewFileUrl,
            Duration = videoDuration
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
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .FirstOrDefaultAsync();

        return post ?? throw new NotFoundException("Video not found");
    }

    public async Task<PaginatedList<PostResponseModel>> GetPostsAsync(PageOption option)
    {
        IQueryable<Post> query = context.Posts
            .Where(p => !p.IsPrivate);

        if (!string.IsNullOrWhiteSpace(option.Search))
        {
            string search = option.Search.Trim();

            query = query.Where(p =>
                EF.Functions.ILike(p.Title, $"%{search}%")); // PostgreSQL case-insensitive
        }

        int postsCount = await query.CountAsync();

        if (postsCount == 0)
           throw new NotFoundException("Video not found");

        List<PostResponseModel> posts = await query
            .OrderBy(p => EF.Functions.Random()) // REAL RANDOM (PostgreSQL)
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        return PaginatedList<PostResponseModel>.Create(
            posts,
            postsCount,
            option.PageNumber,
            option.PageSize
        );
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
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .FirstOrDefaultAsync();

        return post ?? throw new NotFoundException("Video not found");
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
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        return posts.Count == 0 ? throw new NotFoundException("Video not found") : posts;
    }

    public async Task<UpdatePostResponseModel> UpdatePostAsync(Guid id, UpdatePostRequest request)
    {
        Post post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new NotFoundException("Video not found");

        post.Title = request.Title;
        post.Description = request.Description;
        post.PreviewUrl = request.ThumbnailUrl;
        post.IsPrivate = request.IsPrivate;

        context.Posts.Update(post);
        await context.SaveChangesAsync();

        return new UpdatePostResponseModel { Id = post.Id };
    }

    public async Task<DeletePostResponseModel> DeletePostAsync(Guid id)
    {
        // TODO: When delete post need copy to HistoryTable
        Post post = await context.Posts.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("Video not found");

        await fileStorageService.DeleteFileAsync(SystemFolderNames.Video, post.VideoUrl);

        post.IsDeleted = true;
        post.DeletedAt = DateTime.Now;

        await context.SaveChangesAsync();

        return new DeletePostResponseModel("Success");
    }

    public async Task<RestorePostResponseModel> RestorePostAsync(Guid userId)
    {
        Post post = await context.Posts.FirstOrDefaultAsync(u => u.Id == userId)
                    ?? throw new NotFoundException("Video not found");

        post.IsDeleted = false;
        post.DeletedAt = null;

        await context.SaveChangesAsync();

        return new RestorePostResponseModel("Success");
    }
}