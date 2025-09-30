using Microsoft.EntityFrameworkCore;
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

    public PostRepository(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> CreatePost(PostCreateDTO dto)
    {
        int userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

        Post newPost = new Post
        {
            UserId = userId,
            Title = dto.Title,
            Description = dto.Description,
            Duration = dto.Duration,
            PhotoUrl = dto.PhotoUrl,
            VideoUrl = dto.VideoUrl,
            IsPrivate = dto.IsPrivate
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "Post created successfully",
            StatusCode = 201
        };
    }

    public async Task<Result<List<PostGetDTO>>> GetAllPosts()
    {
        List<PostGetDTO> posts = await _context.Posts
            .Where(p => p.IsPrivate == false)
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

    public async Task<Result<PostGetDTO>> GetPostById(int id)
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

    public async Task<Result<PostGetDTO>> SearchPostByQuery(int id)
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

    public async Task<Result<List<PostGetDTO>>> GetUserPosts(int id)
    {
        List<PostGetDTO> posts = await _context.Posts
            .Where(p => p.UserId == id)
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

    public async Task<Result> UpdatePostById(int id, PostUpdateDTO dto)
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
}
