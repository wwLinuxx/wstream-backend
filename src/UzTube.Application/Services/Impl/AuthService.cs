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
        if (!await context.Countries.AnyAsync(c => c.Id == request.CountryId))
            throw new NotFoundException("Country topilmadi");

        User newUser = new()
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            CountryId = request.CountryId
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        return new CreateUserResponseModel { Id = newUser.Id };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserRequest request)
    {
        User? user = request.Login.Contains('@')
            ? await context.Users.FirstOrDefaultAsync(u => u.Email == request.Login)
            : await context.Users.FirstOrDefaultAsync(u => u.Username == request.Login);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Email or Password not correct");

        List<string> permissions = await permissionService.GetUserPermissions(user.Id);
        string token = JwtTokenHandler.GenerateToken(user, permissions, configuration);

        return new LoginResponseModel(user.Email, token);
    }
}