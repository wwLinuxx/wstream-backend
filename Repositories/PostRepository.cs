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

    public Result CreatePost(PostCreateDTO dto)
    {
        int userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

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

        _context.Posts.Add(newPost);
        _context.SaveChanges();

        return new Result
        {
            Message = "Post created successfully",
            StatusCode = 201
        };
    }

    public Result<List<PostViewDTO>> ViewAllPosts()
    {
        List<PostViewDTO> posts = _context.Posts
            .Where(p => p.IsPrivate == false)
            .Select(p => new PostViewDTO
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
            .ToList();

        return new Result<List<PostViewDTO>>
        {
            StatusCode = 200,
            Data = posts
        };
    }
}
