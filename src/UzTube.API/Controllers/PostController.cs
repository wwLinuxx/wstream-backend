using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UzTube.API.Filters;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Application.Services;
using UzTube.Core.Enums;

namespace UzTube.API.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController(
    IPostService postService
) : ControllerBase
{
    [HttpPost]
    //[RequirePermission(SystemPermissions.)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreatePostAsync([FromForm] CreatePostRequest model)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await postService.CreatePostAsync(model)));
    }

    [HttpGet("{id:Guid}")]
    //[RequirePermission()]
    public async Task<IActionResult> GetPostAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await postService.GetPostAsync(id)));
    }

    [HttpPost("get-posts")]
    public async Task<IActionResult> GetPostsAsync([FromBody] PageOption option)
    {
        return Ok(ApiResult<PaginatedList<PostResponseModel>>.Success(
            await postService.GetPostsAsync(option)));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPostByQueryAsync([FromQuery][Required] string query)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await postService.SearchPostAsync(query)));
    }

    // [UserOrAdmin]
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdatePostAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePostRequest dto)
    {
        return Ok(ApiResult<UpdatePostResponseModel>.Success(
            await postService.UpdatePostAsync(id, dto)));
    }

    // [UserOrAdmin]
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeletePostAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<DeletePostResponseModel>.Success(
            await postService.DeletePostAsync(id)));
    }

    [HttpPut("{id:Guid}/restore")]
    public async Task<IActionResult> RestorePostAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<RestorePostResponseModel>.Success(
            await postService.RestorePostAsync(id)));
    }
}