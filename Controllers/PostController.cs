using Microsoft.AspNetCore.Mvc;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models.DTO;

namespace UzTube.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(
        IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [RequirePermission]
    [HttpPost("create-post")]
    public IActionResult CreatePost([FromBody] PostCreateDTO dto)
    {
        return _postRepository.CreatePost(dto);
    }

    [HttpGet()]
    public IActionResult GetAllPosts()
    {
        return _postRepository.GetAllPosts();
    }

    [HttpGet("{id}")]
    public IActionResult GetPostById([FromRoute] int id)
    {
        return _postRepository.GetPostById(id);
    }
    {
        return _postRepository.ViewAllPosts();
    }
}
