using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;
using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;

    public PostRepository(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment env)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _env = env;
    }

    public async Task<Result> CreatePostAsync(PostCreateDTO dto)
    {
        int userId = Convert.ToInt32(
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        );
        HttpRequest? request = _httpContextAccessor.HttpContext?.Request;

        string[] allowedThumbnailExtensions = { ".jpg", ".png" };
        string[] allowedVideoExtensions = { ".mp4" };

        string thumbnailsFolder = Path.Combine(_env.WebRootPath, "uploads", "videos", "thumbnails");
        string videosFolder = Path.Combine(_env.WebRootPath, "uploads", "videos");

        Directory.CreateDirectory(thumbnailsFolder);
        Directory.CreateDirectory(videosFolder);

        if (dto.ThumbnailFile == null || dto.VideoFile == null)
            return new Result { Message = "Thumbnail and Video are required", StatusCode = 400};

        string thumbnailExt = Path.GetExtension(dto.ThumbnailFile.FileName).ToLower();
        string videoExt = Path.GetExtension(dto.VideoFile.FileName).ToLower();

        if (!allowedThumbnailExtensions.Contains(thumbnailExt))
            return new Result
            {
                Message = $"Thumbnail must be one of: {string.Join(", ", allowedThumbnailExtensions)}",
                StatusCode = 400
            };

        if (!allowedVideoExtensions.Contains(videoExt))
            return new Result
            {
                Message = $"Video must be one of: {string.Join(", ", allowedVideoExtensions)}",
                StatusCode = 400
            };

        string thumbnailFileName = $"{Guid.NewGuid()}{thumbnailExt}";
        string videoFileName = $"{Guid.NewGuid()}{videoExt}";

        string thumbnailPath = Path.Combine(thumbnailsFolder, thumbnailFileName);
        string videoPath = Path.Combine(videosFolder, videoFileName);

        using (FileStream stream = new FileStream(thumbnailPath, FileMode.Create))
            await dto.ThumbnailFile.CopyToAsync(stream);

        using (FileStream stream = new FileStream(videoPath, FileMode.Create))
            await dto.VideoFile.CopyToAsync(stream);

        string thumbnailUrl = $"{request?.Scheme}://{request?.Host}/uploads/videos/thumbnails/{thumbnailFileName}";
        string videoUrl = $"{request?.Scheme}://{request?.Host}/uploads/videos/{videoFileName}";

        Post newPost = new Post
        {
            UserId = userId,
            Title = dto.Title,
            Description = dto.Description,
            Duration = "00:12", // TODO: video duration ni hisoblash kerak
            PhotoUrl = thumbnailUrl,
            VideoUrl = videoUrl,
            IsPrivate = dto.IsPrivate
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "Post creted successfully",
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
                Message = "Posts not found",
                StatusCode = 404
            };
        }

        return new Result<List<PostGetDTO>>
        {
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
                Message = "Post not found",
                StatusCode = 404
            };
        }

        return new Result<PostGetDTO>
        {
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
                Message = "Post not found",
                StatusCode = 404
            };
        }

        return new Result<PostGetDTO>
        {
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
                Message = "Posts not found",
                StatusCode = 404
            };
        }

        return new Result<List<PostGetDTO>>
        {
            Message = "From Database",
            StatusCode = 200,
            Data = posts
        };
    }

    public async Task<Result> UpdatePostByIdAsync(int id, PostUpdateDTO dto)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return new Result
            {
                Message = "Post not found",
                StatusCode = 404
            };
        }

        post.Title = dto.Title;
        post.Description = dto.Description;
        post.PhotoUrl = dto.PhotoUrl;
        post.IsPrivate = dto.IsPrivate;

        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "Post updated successfully",
            StatusCode = 200
        };
    }

    public async Task<Result> DeletePostByIdAsync(int id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

        if (post == null)
        {
            return new Result
            {
                Message = "Post not found",
                StatusCode = 404
            };
        }

        post.IsDeleted = true;
        post.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "Post deleted successfully",
            StatusCode = 200
        };
    }

    public async Task<Result> RestorePostByIdAsync(int id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

        if (post == null)
        {
            return new Result
            {
                Message = "Post not found",
                StatusCode = 404
            };
        }

        post.IsDeleted = false;
        post.DeletedAt = null;

        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "Post restored successfully",
            StatusCode = 200
        };
    }
}
