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
    private readonly IWebHostEnvironment _env;

    public PostController(
        IPostRepository postRepository,
        IWebHostEnvironment env)
    {
        _postRepository = postRepository;
        _env = env;
    }

    [RequirePermission(SystemPermissions.Authorize)]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePost([FromForm] PostCreateDTO dto)
    {
        return await _postRepository.CreatePostAsync(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        return await _postRepository.GetAllPostsAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        return await _postRepository.GetPostByIdAsync(id);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPostByQuery([FromQuery] [Required] int id)
    {
        return await _postRepository.SearchPostByQueryAsync(id);
    }

    [UserOrAdmin]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePostById(
        [FromRoute] int id,
        [FromBody] PostUpdateDTO dto)
    {
        return await _postRepository.UpdatePostByIdAsync(id, dto);
    }

    [UserOrAdmin]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostById([FromRoute] int id)
    {
        return await _postRepository.DeletePostByIdAsync(id);
    }


    [HttpPut("{id}/restore")]
    public async Task<IActionResult> RestorePostById([FromRoute] int id)
    {
        return await _postRepository.RestorePostByIdAsync(id);
    }
}
