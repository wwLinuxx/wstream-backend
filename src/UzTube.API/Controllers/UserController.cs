using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UzTube.API.Filters;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Application.Models.User;
using UzTube.Application.Services;
using UzTube.Core.Enums;

namespace UzTube.API.Controllers;

public class UserController(
    IUserService userService,
    IPostService postService
) : ApiController
{
    // [RequirePermission(SystemPermissions.ManageUsers)]
    [HttpPost("get-users")]
    public async Task<IActionResult> GetListUsersAsync([FromBody] PageOption option)
    {
        return Ok(ApiResult<PaginatedList<UserResponseModel>>.Success(
            await userService.GetUsersAsync(option)));
    }

    // [UserOrAdmin]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetUserProfileByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.GetUserProfileByIdAsync(id)));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUserByQueryAsync([FromQuery] [Required] string query)
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.SearchUserByQueryAsync(query)));
    }

    // [UserOrAdmin]
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateUserProfileByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserProfileModel model)
    {
        return Ok(ApiResult<UpdateUserProfileResponseModel>.Success(
            await userService.UpdateUserProfileByIdAsync(id, model)));
    }

    // [UserOrAdmin]
    [HttpPut("{id:Guid}/password")]
    public async Task<IActionResult> UpdateUserPasswordByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserPasswordModel model)
    {
        return Ok(ApiResult<UpdateUserPasswordResponseModel>.Success(
            await userService.UpdateUserPasswordByIdAsync(id, model)));
    }

    // [RequirePermission(SystemPermissions.ManageRoles)]
    [HttpPut("{id:Guid}/roles")]
    public async Task<IActionResult> UpdateUserRoleByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRoleModel model)
    {
        return Ok(ApiResult<UpdateUserRoleResponseModel>.Success(
            await userService.UpdateUserRoleByIdAsync(id, model)));
    }

    // [UserOrAdmin]
    [HttpGet("{id:Guid}/posts")]
    public async Task<IActionResult> GetUserPostsAsync(
        [FromRoute] Guid id,
        [FromBody] PageOption option)
    {
        return Ok(ApiResult<List<PostResponseModel>>.Success(
            await postService.GetUserPostsAsync(id, option)));
    }

    // [RequirePermission(SystemPermissions.ManageUsers)]
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<DeleteUserResponseModel>.Success(
            await userService.DeleteUserByIdAsync(id)));
    }

    // [RequirePermission(SystemPermissions.ManageUsers)]
    [HttpPut("{id:Guid}/restore")]
    public async Task<IActionResult> RestoreUserByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<RestoreUserResponseModel>.Success(
            await userService.RestoreUserByIdAsync(id)));
    }
}