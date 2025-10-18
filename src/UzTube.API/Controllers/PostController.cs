using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using UzTube.API.Controllers;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Controllers;

public class PostController : ApiController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPostService _service;

    public PostController(
        IHttpContextAccessor httpContextAccessor,
        IPostService postService)
    {
        _httpContextAccessor = httpContextAccessor;
        _service = postService;
    }

    [RequirePermission(SystemPermissions.Authorize)]
    [HttpPost]
    public async Task<IActionResult> CreatePostAsync([FromForm] CreatePostModel dto)
    {
        Guid userId = Guid.Parse(
            _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
        );

        return Ok(ApiResult<CreatePostResponseModel>.Success(
            await _service.CreatePostAsync(dto, userId)));
    }
      
    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
        return Ok(ApiResult<List<PostResponseModel>>.Success(
            await _service.GetPostsAsync()));
    }

    [HttpPost("get-posts-list")]
    public async Task<IActionResult> GetListPostsAsync([FromBody] PageOption option)
    {
        return Ok(ApiResult<PaginatedList<PostListResonseModel>>.Success(
            await _service.GetListPostsAsync(option)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await _service.GetPostByIdAsync(id)));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPostByQueryAsync([FromQuery] [Required] Guid id)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await _service.SearchPostByQueryAsync(id)));
    }

    [UserOrAdmin]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePostByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePostModel dto)
    {
        return Ok(ApiResult<UpdatePostResponseModel>.Success(
            await _service.UpdatePostByIdAsync(id, dto)));
    }

    [UserOrAdmin]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<DeletePostResponseModel>.Success(
            await _service.DeletePostByIdAsync(id)));
    }

    [HttpPut("{id}/restore")]
    public async Task<IActionResult> RestorePostByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<RestorePostResponseModel>.Success(
            await _service.RestorePostByIdAsync(id)));
    }
}
