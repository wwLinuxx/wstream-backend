using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Repositories;

public class PostService : IPostService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileUploadService _fileUploadService;

    public PostService(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IFileUploadService fileUploadService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _fileUploadService = fileUploadService;
    }

    public async Task<ApiResult> CreatePostAsync(PostCreateDTO dto)
    {
        int userId = Convert.ToInt32(
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        );

        Result<string> uploadThumbnailResult = await _fileUploadService.UploadThumbnail(dto.ThumbnailFile);
        Result<string> uploadVideoResult = await _fileUploadService.UploadVideo(dto.VideoFile);
        
        if (!uploadThumbnailResult.Succeed)
            return new ApiResult
            {
                Succeed = false,
                Message = uploadThumbnailResult.Message,
                StatusCode = uploadThumbnailResult.StatusCode
            };

        if (!uploadVideoResult.Succeed)
            return new ApiResult
            {
                Succeed = false,
                Message = uploadVideoResult.Message,
                StatusCode = uploadVideoResult.StatusCode
            };

        string thumbnailUrl = uploadThumbnailResult.Data;
        string videoUrl = uploadVideoResult.Data;

        Post newPost = new Post
        {
            UserId = userId,
            Title = dto.Title,
            Description = dto.Description,
            Duration = "00:12", // TODO: video duration hisoblash
            PhotoUrl = thumbnailUrl,
            VideoUrl = videoUrl,
            IsPrivate = dto.IsPrivate
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "Post created successfully",
            StatusCode = 201
        };
    }

    public async Task<Result<List<PostGetDTO>>> GetAllPostsAsync()
    {
        List<PostGetDTO> posts = await _context.Posts
            .Where(p => p.IsPrivate == false && p.IsDeleted == false)
            .OrderBy(p => p.Id)
            .Select(p => new PostGetDTO
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
        {
            return new Result<List<PostGetDTO>>
            {
                Succeed = false,
                Message = "Posts not found",
                StatusCode = 404
            };
        }

        return new Result<List<PostGetDTO>>
        {
            Succeed = true,
            StatusCode = 200,
            Data = posts
        };
    }

    public async Task<Result<PostGetDTO>> GetPostByIdAsync(int id)
    {
        PostGetDTO? post = await _context.Posts
            .Where(p => p.Id == id)
            .Select(p => new PostGetDTO
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
        {
            return new Result<PostGetDTO>
            {
                Succeed = false,
                Message = "Post not found",
                StatusCode = 404
            };
        }

        return new Result<PostGetDTO>
        {
            Succeed = true,
            Message = "From Database",
            StatusCode = 200,
            Data = post
        };
    }

    public async Task<Result<PostGetDTO>> SearchPostByQueryAsync(int id)
    {
        PostGetDTO? post = await _context.Posts
            .Where(p => p.Id == id && p.IsPrivate == false)
            .Select(p => new PostGetDTO
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
        {
            return new Result<PostGetDTO>
            {
                Succeed = false,
                Message = "Post not found",
                StatusCode = 404
            };
        }

        return new Result<PostGetDTO>
        {
            Succeed = true,
            Message = "From Database",
            StatusCode = 200,
            Data = post
        };
    }

    public async Task<Result<List<PostGetDTO>>> GetUserPostsAsync(int id)
    {
        List<PostGetDTO> posts = await _context.Posts
            .Where(p => p.UserId == id && p.IsDeleted == false)
            .OrderBy(p => p.Id)
            .Select(p => new PostGetDTO
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
        {
            return new Result<List<PostGetDTO>>
            {
                Succeed = false,
                Message = "Posts not found",
                StatusCode = 404
            };
        }

        return new Result<List<PostGetDTO>>
        {
            Succeed = true,
            Message = "From Database",
            StatusCode = 200,
            Data = posts
        };
    }

    public async Task<ApiResult> UpdatePostByIdAsync(int id, PostUpdateDTO dto)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "Post not found",
                StatusCode = 404
            };
        }

        post.Title = dto.Title;
        post.Description = dto.Description;
        post.PhotoUrl = dto.PhotoUrl;
        post.IsPrivate = dto.IsPrivate;

        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "Post updated successfully",
            StatusCode = 200
        };
    }

    public async Task<ApiResult> DeletePostByIdAsync(int id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

        if (post == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "Post not found",
                StatusCode = 404
            };
        }

        post.IsDeleted = true;
        post.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "Post deleted successfully",
            StatusCode = 200
        };
    }

    public async Task<ApiResult> RestorePostByIdAsync(int id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

        if (post == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "Post not found",
                StatusCode = 404
            };
        }

        post.IsDeleted = false;
        post.DeletedAt = null;

        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "Post restored successfully",
            StatusCode = 200
        };
    }
}
