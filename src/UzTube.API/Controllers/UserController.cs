using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UzTube.Application.Models;
using UzTube.Application.Models.User;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IPostService _postService;

    public UserController(
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        IPostService postService
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _postService = postService;
    }

    [RequirePermission(SystemPermissions.ViewUsers)]
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
        return Ok(ApiResult<List<UserResponseModel>>.Success(
            await _userService.GetAllUsersAsync()));
    }

    [RequirePermission(SystemPermissions.ViewUsers)]
    [HttpPost("get-users-list")]
    public async Task<IActionResult> GetListUsersAsync([FromBody] PageOption option)
    {
        return Ok(ApiResult<PaginationResult<UserListResponseModel>>.Success(
            await _userService.GetAllUsersAsync(option)));
    }

    [UserOrAdmin]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetUserProfileByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await _userService.GetUserProfileByIdAsync(id)));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUserByQueryAsync([FromQuery][Required] Guid id)
        => Ok(ApiResult<UserResponseModel>.Success(
            await _userService.SearchUserByQueryAsync(id)));

    [UserOrAdmin]
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateUserProfileByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserProfileModel dto)
    {
        return Ok(ApiResult<UpdateUserProfileResponseModel>.Success(
            await _userService.UpdateUserProfileByIdAsync(id, dto)));
    }

    [UserOrAdmin]
    [HttpPut("{id:Guid}/password")]
    public async Task<IActionResult> UpdateUserPasswordByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserPasswordModel dto)
    {
        return Ok(ApiResult<UpdateUserPasswordResponseModel>.Success(
            await _userService.UpdateUserPasswordByIdAsync(id, dto)));
    }

    [RequirePermission(SystemPermissions.ManageRoles)]
    [HttpPut("{id:Guid}/roles")]
    public async Task<IActionResult> UpdateUserRoleByIdAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRoleModel dto)
    {
        return Ok(ApiResult<UpdateUserRoleResponseModel>.Success(
            await _userService.UpdateUserRoleByIdAsync(id, dto)));
    }

    [UserOrAdmin]
    [HttpGet("{id:Guid}/posts")]
    public async Task<IActionResult> GetUserPostsAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<List<PostResponseModel>>.Success(
            await _postService.GetUserPostsAsync(id)));
    }

    [RequirePermission(SystemPermissions.ManageUser)]
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<DeleteUserResonseModel>.Success(
            await _userService.DeleteUserByIdAsync(id)));
    }

    [RequirePermission(SystemPermissions.ManageUser)]
    [HttpPut("{id:Guid}/restore")]
    public async Task<IActionResult> RestoreUserByIdAsync([FromRoute] Guid id)
    {
        return Ok(ApiResult<RestoreUserResponseModel>.Success(
            await _userService.RestoreUserByIdAsync(id)));
    }
}
