using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exeptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.DataAccess.Persistence;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models.DTO;

namespace UzTube.Application.Services.Impl;

public class PostService : IPostService
{
    private readonly DatabaseContext _context;

    public PostService(
        DatabaseContext context)
    {
        _context = context;
    }

    public async Task<CreatePostResponseModel> CreatePostAsync(CreatePostModel dto, Guid id) // TODO: Create Post Min.io ni o'rnatish
    {
        Post newPost = new Post
        {
            UserId = id,
            Title = dto.Title,
            Description = dto.Description,
            Duration = "00:12", // TODO: video duration hisoblash
            PhotoUrl = dto.ThumbnailFile,
            VideoUrl = dto.VideoFile,
            IsPrivate = dto.IsPrivate
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return new CreatePostResponseModel { Id = id };
    }
    
    public async Task<List<PostResponseModel>> GetPostsAsync()
    {
        List<PostResponseModel> posts = await _context.Posts
            .Where(p => p.IsPrivate == false && p.IsDeleted == false)
            //.OrderBy(p => p.Id)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.PhotoUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedAt = p.PostedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        if (posts.Count == 0)
            throw new NotFoundException("Post not found");

        return posts;
    }

    public async Task<PaginatedList<PostListResonseModel>> GetListPostsAsync(PageOption option)
    {
        List<PostListResonseModel> posts = await _context.Posts
            .Skip(option.PageSize * (option.PageNumber - 1))
            .Take(option.PageSize)
            .Where(p => p.IsPrivate == false && p.IsDeleted == false)
            //.OrderBy(p => p.Id)
            .Select(p => new PostListResonseModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.PhotoUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedAt = p.PostedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        if (posts.Count == 0)
            throw new NotFoundException("Post not found");

        return new PaginatedList<PostListResonseModel>
        {
            Items = posts,
            TotalPages = option.PageSize,
            PageNumber = option.PageNumber,
            TotalCount = posts.Count
        };
    }

    public async Task<PostResponseModel> GetPostByIdAsync(Guid id)
    {
        PostResponseModel? post = await _context.Posts
            .Where(p => p.Id == id)
            .Select(p => new PostResponseModel
            {
                Id = id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.PhotoUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedAt = p.PostedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .FirstOrDefaultAsync();

        if (post == null)
            throw new NotFoundException("Post not found");

        return post;
    }

    public async Task<PostResponseModel> SearchPostByQueryAsync(Guid id)
    {
        PostResponseModel? post = await _context.Posts
            .Where(p => p.Id == id && p.IsPrivate == false)
            .Select(p => new PostResponseModel
            {
                Id = id,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.PhotoUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedAt = p.PostedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .FirstOrDefaultAsync();

        if (post == null)
            throw new NotFoundException("Post not found");

        return post;
    }

    public async Task<List<PostResponseModel>> GetUserPostsAsync(Guid id)
    {
        List<PostResponseModel> posts = await _context.Posts
            .Where(p => p.UserId == id && p.IsDeleted == false)
            .OrderBy(p => p.Id)
            .Select(p => new PostResponseModel
            {
                Id = p.Id,
                UserId = id,
                Title = p.Title,
                Description = p.Description,
                PhotoUrl = p.PhotoUrl,
                VideoUrl = p.VideoUrl,
                Duration = p.Duration,
                PostedAt = p.PostedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                ViewsCount = p.ViewsCount,
                LikesCount = p.LikesCount,
                Rating = p.Rating,
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();

        if (posts.Count == 0)
            throw new NotFoundException("Post not found");

        return posts;
    }

    public async Task<UpdatePostResponseModel> UpdatePostByIdAsync(Guid id, UpdatePostModel dto)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
            throw new NotFoundException("Post not found");

        post.Title = dto.Title;
        post.Description = dto.Description;
        post.PhotoUrl = dto.PhotoUrl;
        post.IsPrivate = dto.IsPrivate;

        _context.Posts.Update(post);
        await _context.SaveChangesAsync();

        return new UpdatePostResponseModel { Id = post.Id };
    }

    public async Task<DeletePostResponseModel> DeletePostByIdAsync(Guid id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

        if (post == null)
            throw new NotFoundException("Post not found");

        post.IsDeleted = true;
        post.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new DeletePostResponseModel { Result = "Success" };
    }

    public async Task<RestorePostResponseModel> RestorePostByIdAsync(Guid id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

        if (post == null)
            throw new NotFoundException("Post not found");

        post.IsDeleted = false;
        post.DeletedAt = null;

        await _context.SaveChangesAsync();

        return new RestorePostResponseModel { Result = "Success" };
    }
}
