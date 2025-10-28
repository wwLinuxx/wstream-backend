using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Application.Services;

namespace UzTube.API.Controllers;

public class PostController(
    IPostService postService
) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreatePostAsync([FromForm] CreatePostModel model)
    {
        return Ok(ApiResult<CreatePostResponseModel>.Success(
            await postService.CreatePostAsync(model)));
    }

    [HttpPost("get-posts")]
    public async Task<IActionResult> GetListPostsAsync([FromBody] PageOption option)
    {
        return Ok(ApiResult<PaginatedList<PostResponseModel>>.Success(
            await postService.GetPostsAsync(option)));
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await postService.GetPostByIdAsync(id)));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPostByQueryAsync([FromQuery] [Required] string query)
    {
        return Ok(ApiResult<PostResponseModel>.Success(
            await postService.SearchPostByQueryAsync(query)));
    }

    // [UserOrAdmin]
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdatePostByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePostModel dto)
    {
        return Ok(ApiResult<UpdatePostResponseModel>.Success(
            await postService.UpdatePostByIdAsync(id, dto)));
    }

    // [UserOrAdmin]
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeletePostByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<DeletePostResponseModel>.Success(
            await postService.DeletePostByIdAsync(id)));
    }

    [HttpPut("{id:Guid}/restore")]
    public async Task<IActionResult> RestorePostByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<RestorePostResponseModel>.Success(
            await postService.RestorePostByIdAsync(id)));
    }
}