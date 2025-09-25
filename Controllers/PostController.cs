using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    public IActionResult CreatePost([FromBody] PostCreateDTO dto)
    {
        return _postRepository.CreatePost(dto);
    }
}
