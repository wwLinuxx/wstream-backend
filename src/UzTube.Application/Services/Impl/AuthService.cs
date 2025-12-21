using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UzTube.Application.Exceptions;
using UzTube.Application.Helpers.GenerateJwt;
using UzTube.Shared.Helpers.Interfaces;
using UzTube.Application.Models.User;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Services.Impl;

public class AuthService(
    DatabaseContext context,
    IPermissionService permissionService,
    IPasswordHasher passwordHasher,
    IConfiguration configuration
) : IAuthService
{
    public async Task<CreateUserResponseModel> CreateAsync(CreateUserRequest request)
    {
        string hashedPassword = passwordHasher.Hash(request.Password);

        User newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword,
            CountryId = request.CountryId,
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return new CreateUserResponseModel { Id = newUser.Id };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserRequest request)
    {
        User user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Login)
            ?? throw new NotFoundException("User not found");

        if (!passwordHasher.Verify(user.PasswordHash, request.Password))
            throw new BadRequestException("Email or Password not correct");

        List<string> permissions = await permissionService.GetUserPermissions(user.Id);

        string token = JwtTokenHandler.GenerateToken(user, permissions, configuration);

        return new LoginResponseModel(user.Email, token);
    }
}