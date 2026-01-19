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
    DatabaseContext db,
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

    public async Task<PostResponseModel> CreatePostAsync(CreatePostRequest request)
    {
        Guid userId = claimService.GetUserId();

        string? previewFileUrl = null;

        string videoFileUrl = await fileStorageService.UploadVideoFileAsync(request.VideoFile);

        if (request.PreviewFile != null)
            previewFileUrl = await fileStorageService.UploadPreviewFileAsync(request.PreviewFile);

        string videoDuration = "24:12:60"; // TODO: Video ni vaqtini hisoblash

        Post newPost = new Post
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            PreviewUrl = previewFileUrl,
            VideoUrl = videoFileUrl,
            Duration = videoDuration,
            IsPrivate = request.IsPrivate
        };

        await db.Posts.AddAsync(newPost);
        await db.SaveChangesAsync();

        return await GetPostAsync(newPost.Id);
    }

    public async Task<PostResponseModel> GetPostAsync(Guid id)
    {
        PostResponseModel post = await db.Posts
            .Where(p => p.Id == id)
            .Include(p => p.User)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate,
                UserId = p.UserId,
                Username = p.User.Username,
                UserAvatarUrl = p.User.AvatarUrl
            })
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Video not found");

        return post;
    }

    public async Task<PaginatedList<PostResponseModel>> GetPostsAsync(PageOption option)
    {
        IQueryable<Post> query = db.Posts.Where(p => !p.IsPrivate);

        if (!string.IsNullOrWhiteSpace(option.Search))
        {
            string search = option.Search.Trim();
            query = query.Where(p => EF.Functions.ILike(p.Title, $"%{search}%"));
        }

        int totalCount = await query.CountAsync();

        if (totalCount == 0)
            return PaginatedList<PostResponseModel>.Empty(option.PageNumber, option.PageSize);

        List<PostResponseModel> posts = await query
            .OrderByDescending(p => EF.Functions.Random())
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate,
                UserId = p.UserId,
                Username = p.User.Username,
                UserAvatarUrl = p.User.AvatarUrl
            })
            .ToListAsync();

        return PaginatedList<PostResponseModel>.Create(posts, totalCount, option.PageNumber, option.PageSize);
    }

    public async Task<PostResponseModel> SearchPostAsync(string query)
    {
        if (!string.IsNullOrEmpty(query))
            query = query.Trim();

        PostResponseModel? post = await db.Posts
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

    public async Task<PaginatedList<PostResponseModel>> GetUserPostsAsync(Guid userId, PageOption option)
    {
        IQueryable<Post> query = db.Posts.Where(p => p.UserId == userId && !p.IsPrivate);

        if (!string.IsNullOrWhiteSpace(option.Search))
        {
            string search = option.Search.Trim();
            query = query.Where(p => EF.Functions.ILike(p.Title, $"%{search}%"));
        }

        int totalCount = await query.CountAsync();

        if (totalCount == 0)
            return PaginatedList<PostResponseModel>.Empty(option.PageNumber, option.PageSize);

        List<PostResponseModel> posts = await query
            .OrderByDescending(p => p.PostedOn)
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                PreviewUrl = p.PreviewUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedOn = p.PostedOn,
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate,
                UserId = p.UserId,
                Username = p.User.Username,
                UserAvatarUrl = p.User.AvatarUrl
            })
            .ToListAsync();

        return PaginatedList<PostResponseModel>.Create(posts, totalCount, option.PageNumber, option.PageSize);
    }

    public async Task<UpdatePostResponseModel> UpdatePostAsync(Guid id, UpdatePostRequest request)
    {
        Post post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new NotFoundException("Video not found");

        string? previewFileUrl = null;

        if (request.PreviewFile != null)
            previewFileUrl = await fileStorageService.UploadPreviewFileAsync(request.PreviewFile);

        post.Title = request.Title;
        post.Description = request.Description;
        post.PreviewUrl = previewFileUrl;
        post.IsPrivate = request.IsPrivate;

        db.Posts.Update(post);
        await db.SaveChangesAsync();

        return new UpdatePostResponseModel { Id = post.Id };
    }

    public async Task<DeletePostResponseModel> DeletePostAsync(Guid id)
    {
        // TODO: When delete post need copy to HistoryTable
        Post post = await db.Posts.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("Video not found");

        await fileStorageService.DeleteFileAsync(SystemFolderNames.Video, post.VideoUrl);

        post.IsDeleted = true;
        post.DeletedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return new DeletePostResponseModel("Success");
    }

    public async Task<RestorePostResponseModel> RestorePostAsync(Guid userId)
    {
        Post post = await db.Posts.FirstOrDefaultAsync(u => u.Id == userId)
                    ?? throw new NotFoundException("Video not found");

        post.IsDeleted = false;
        post.DeletedAt = null;

        await db.SaveChangesAsync();

        return new RestorePostResponseModel("Success");
    }
}