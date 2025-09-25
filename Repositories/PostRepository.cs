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
}
