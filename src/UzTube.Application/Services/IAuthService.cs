using UzTube.Application.Models.User;

namespace UzTube.Application.Services;

public interface IAuthService
{
    Task<CreateUserResponseModel> CreateAsync(CreateUserRequest request);
    Task<LoginResponseModel> LoginAsync(LoginUserRequest request);
}