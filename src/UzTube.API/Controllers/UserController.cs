using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Application.Models.User;
using UzTube.Application.Services;

namespace UzTube.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(
    IUserService userService,
    IPostService postService
) : ControllerBase
{
    // [UserOrAdmin]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetUserAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.GetUserAsync(id)));
    }

    [HttpGet]
    public async Task<IActionResult> GetUserAsync([FromQuery] string username)
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.GetUserAsync(username)));
    }

    // [RequirePermission(SystemPermissions.ManageUsers)]
    [HttpPost("get-users")]
    public async Task<IActionResult> GetUsersAsync([FromBody] PageOption option)
    {
        return Ok(ApiResult<PaginatedList<UserResponseModel>>.Success(
            await userService.GetUsersAsync(option)));
    }

    [HttpGet("preview/{userId:Guid}")]
    //[RequirePermission(SystemPermissions.PostView)]
    public async Task<IActionResult> GetUserPreviewAsync([FromRoute] Guid userId)
    {
        return Ok(ApiResult<UserPreviewResponseModel>.Success(
            await userService.GetUserPreviewAsync(userId)));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUserAsync([FromQuery][Required] string query)
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.SearchUserAsync(query)));
    }

    // [UserOrAdmin]
    [HttpPut("{id:Guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateUserAsync(
        [FromRoute] Guid id,
        [FromForm] UpdateUserRequest model)
    {
        return Ok(ApiResult<UpdateUserProfileResponseModel>.Success(
            await userService.UpdateUserProfileAsync(id, model)));
    }

    // [UserOrAdmin]
    [HttpPut("{id:Guid}/password")]
    public async Task<IActionResult> UpdateUserPasswordAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserPasswordRequest request)
    {
        return Ok(ApiResult<UpdateUserPasswordResponseModel>.Success(
            await userService.UpdateUserPasswordAsync(id, request)));
    }

    // [RequirePermission(SystemPermissions.ManageRoles)]
    [HttpPut("{id:Guid}/roles")]
    public async Task<IActionResult> UpdateUserRolesAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRolesRequest model)
    {
        return Ok(ApiResult<UpdateUserRoleResponseModel>.Success(
            await userService.UpdateUserRolesAsync(id, model)));
    }

    // [UserOrAdmin]
    [HttpPost("{id:Guid}/posts")]
    public async Task<IActionResult> GetUserPostsAsync(
        [FromRoute] Guid id,
        [FromBody] PageOption option)
    {
        return Ok(ApiResult<List<PostResponseModel>>.Success(
            await postService.GetUserPostsAsync(id, option)));
    }

    // [RequirePermission(SystemPermissions.ManageUsers)]
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<DeleteUserResponseModel>.Success(
            await userService.DeleteUserAsync(id)));
    }

    // [RequirePermission(SystemPermissions.ManageUsers)]
    [HttpPut("{id:Guid}/restore")]
    public async Task<IActionResult> RestoreUserAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<RestoreUserResponseModel>.Success(
            await userService.RestoreUserAsync(id)));
    }
}