// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using UzTube.Application.Models;
// using UzTube.Application.Models.OtpCode;
// using UzTube.Application.Models.User;
// using UzTube.Application.Services;
//
// namespace UzTube.API.Controllers;
//
// [ApiController]
// [Route("api/auth")]
// public class AuthController(
//     IUserService userService,
//     IOtpService otpService
// ) : ControllerBase
// {
//     [HttpPost("register")]
//     public async Task<IActionResult> CreateAsync(CreateUserRequest request)
//     {
//         return Ok(ApiResult<CreateUserResponseModel>.Success(
//             await userService.CreateAsync(request)));
//     }
//
//     [HttpPost("login")]
//     public async Task<IActionResult> LoginAsync(LoginUserRequest request)
//     {
//         return Ok(ApiResult<LoginResponseModel>.Success(
//             await userService.LoginAsync(request)));
//     }
//
//     [Authorize]
//     [HttpGet("me")]
//     public async Task<IActionResult> GetMeAsync()
//     {
//         return Ok(ApiResult<UserResponseModel>.Success(
//             await userService.GetMeAsync()));
//     }
//
//     [HttpPost("send-otp/{userId:Guid}")]
//     public async Task<IActionResult> SendOtpAsync(Guid userId)
//     {
//         return Ok(ApiResult<OtpResponseModel>.Success(
//             await otpService.SendOtpAsync(userId)));
//     }
//
//     [HttpPost("verify-otp/{userId:Guid}")]
//     public async Task<IActionResult> VerifyOtpAsync(
//         [FromRoute] Guid userId,
//         [FromQuery] string otpCode)
//     {
//         return Ok(ApiResult<OtpResponseModel>.Success(
//             await otpService.VerifyOtpAsync(userId, otpCode)));
//     }
// }