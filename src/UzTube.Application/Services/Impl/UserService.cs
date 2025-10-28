using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UzTube.Application.Exeptions;
using UzTube.Application.Helpers;
using UzTube.Application.Helpers.Interfaces;
using UzTube.Application.Models;
using UzTube.Application.Models.User;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class UserService(
    DatabaseContext context,
    IPermissionService permissionService,
    IClaimService claimService,
    IConfiguration configuration,
    IPasswordHelper passwordHelper
) : IUserService
{
    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel model)
    {
        var salt = passwordHelper.GenerateSalt();
        var passwordHash = passwordHelper.Encrypt(model.Password, salt);

        var newUser = new User
        {
            Email = model.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            Profile = new UserProfile
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                PhoneNumber = model.PhoneNumber,
                CountryId = model.CountryId
            }
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return new CreateUserResponseModel { Id = newUser.Id };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserModel model)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email)
                   ?? throw new BadRequestException("User not found");

        if (!passwordHelper.Verify(user.PasswordHash, model.Password, user.Salt))
            throw new BadRequestException("Email or Password not correct");

        var permissions = await permissionService.GetUserPermissions(user.Id);

        var token = JwtHelper.GenerateToken(user, permissions, configuration);

        return new LoginResponseModel(user.Email, token);
    }

    public async Task<UserResponseModel> GetMeAsync()
    {
        var userId = claimService.GetUserId();

        var user = await context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserResponseModel
            {
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryCode = u.Profile.Country.Code,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<PaginatedList<UserResponseModel>> GetUsersAsync(PageOption option)
    {
        IQueryable<User> query = context.Users;

        if (!string.IsNullOrEmpty(option.Search))
            query = query.Where(u =>
                u.Profile.FirstName.Contains(option.Search.Trim(), StringComparison.OrdinalIgnoreCase));

        var users = await query
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryCode = u.Profile.Country.FullName,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .ToListAsync();

        if (users.Count == 0)
            throw new NotFoundException("Users not found");

        var pagesCount = await context.Posts.CountAsync();

        return PaginatedList<UserResponseModel>.Create(users, pagesCount, option.PageNumber, option.PageSize);
    }

    public async Task<UserResponseModel> GetUserProfileByIdAsync(Guid id)
    {
        var user = await context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryCode = u.Profile.Country.FullName,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UserResponseModel> SearchUserByQueryAsync(string query)
    {
        if (!string.IsNullOrEmpty(query))
            query = query.Trim();

        var user = await context.Users
            .Include(u => u.Profile)
            .Where(u => u.Profile.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryCode = u.Profile.Country.FullName,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UpdateUserProfileResponseModel> UpdateUserProfileByIdAsync(Guid id, UpdateUserProfileModel model)
    {
        var user = await context.Users
                       .Include(u => u.Profile)
                       .ThenInclude(p => p.Country)
                       .FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException("User not found");

        user.Profile.FirstName = model.FirstName;
        user.Profile.LastName = model.LastName;
        user.Profile.PhoneNumber = model.PhoneNumber;
        user.Profile.Age = model.Age;
        user.Profile.CountryId = model.CountryId;

        user.UpdatedOn = DateTime.Now;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserProfileResponseModel { Id = user.Id };
    }

    public async Task<UpdateUserPasswordResponseModel> UpdateUserPasswordByIdAsync(Guid id, UpdateUserPasswordModel model)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException("User not found");

        if (!passwordHelper.Verify(user.PasswordHash, model.OldPassword, user.Salt))
            throw new BadRequestException("Old Password not correct");

        if (model.NewPassword != model.ConfirmPassword)
            throw new BadRequestException("NewPassword and ConfirmPassword not equal");

        var newSalt = passwordHelper.GenerateSalt();
        var newHashedPassword = passwordHelper.Encrypt(model.NewPassword, newSalt);

        user.Salt = newSalt;
        user.PasswordHash = newHashedPassword;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserPasswordResponseModel { Id = id };
    }

    public async Task<UpdateUserRoleResponseModel> UpdateUserRoleByIdAsync(Guid id, UpdateUserRoleModel model)
    {
        var user = await context.Users
                       .Include(u => u.Roles)
                       .FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException("User not found");

        user.Roles = model.Roles
            .Select(rid => new UserRole
            {
                RoleId = rid
            })
            .ToList();

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserRoleResponseModel { Id = id };
    }

    public async Task<DeleteUserResponseModel> DeleteUserByIdAsync(Guid id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException("User not found");

        user.IsDeleted = true;
        user.DeletedOn = DateTime.Now;

        await context.SaveChangesAsync();

        return new DeleteUserResponseModel("Success");
    }

    public async Task<RestoreUserResponseModel> RestoreUserByIdAsync(Guid id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException("User not found");

        user.IsDeleted = false;
        user.DeletedOn = null;

        await context.SaveChangesAsync();

        return new RestoreUserResponseModel { Id = id };
    }
}