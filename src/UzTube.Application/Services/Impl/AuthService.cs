using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UzTube.Application.Exceptions;
using UzTube.Application.Helpers.GenerateJwt;
using UzTube.Application.Helpers.Interfaces;
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
        string salt = passwordHasher.GenerateSalt();
        string passwordHash = passwordHasher.Encrypt(request.Password, salt);

        User newUser = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            Profile = new Profile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                PhoneNumber = request.PhoneNumber,
                CountryId = request.CountryId
            }
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return new CreateUserResponseModel { Id = newUser.Id };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserRequest request)
    {
        User user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
                    ?? throw new NotFoundException("User not found");

        if (!passwordHasher.Verify(user.PasswordHash, request.Password, user.Salt))
            throw new BadRequestException("Email or Password not correct");

        List<string> permissions = await permissionService.GetUserPermissions(user.Id);

        string token = JwtTokenHandler.GenerateToken(user, permissions, configuration);

        return new LoginResponseModel(user.Email, token);
    }
}