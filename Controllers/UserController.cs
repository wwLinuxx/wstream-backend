using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models.DTO;

namespace UzTube.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public UserController(
            IUserRepository userRepository,
            IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [RequirePermission(SystemPermissions.ViewUsers)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        [UserOrAdmin]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileById([FromRoute] int id)
        {
            return await _userRepository.GetUserProfileById(id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUserByQuery([FromQuery] [Required] int id)
        {
            return await _userRepository.SearchUserByQuery(id);
        }

        [UserOrAdmin]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfileById(
            [FromRoute] int id,
            [FromBody] UserProfileUpdateDTO dto)
        {
            return await _userRepository.UpdateUserProfileById(id, dto);
        }

        [UserOrAdmin]
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPasswordById(
            [FromRoute] int id,
            [FromBody] UserPasswordUpdateDTO dto)
        {
            return await _userRepository.UpdateUserPasswordById(id, dto);
        }

        [RequirePermission(SystemPermissions.ManageRoles)]
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateUserRoleById(
            [FromRoute] int id,
            [FromBody] UserRoleUpdateDTO dto)
        {
            return await _userRepository.UpdateUserRoleById(id, dto);
        }

        [UserOrAdmin]
        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetUserPosts([FromRoute] int id)
        {
            return await _postRepository.GetUserPostsAsync(id);
        }

        [RequirePermission(SystemPermissions.ManageUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById([FromRoute] int id)
        {
            return await _userRepository.DeleteUserById(id);
        }

        [RequirePermission(SystemPermissions.ManageUser)]
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreUserById([FromRoute] int id)
        {
            return await _userRepository.RestoreUserById(id);
        }
    }
}
