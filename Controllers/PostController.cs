using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

    [RequirePermission(SystemPermissions.Authorize)]
    [HttpPost("create-post")]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateDTO dto)
    {
        return await _postRepository.CreatePost(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        return await _postRepository.GetAllPosts();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        return await _postRepository.GetPostById(id);
    }

    [HttpGet("/search")]
    public async Task<IActionResult> SearchPostByQuery([FromQuery] [Required] int id)
    {
        return await _postRepository.SearchPostByQuery(id);
    }
}
