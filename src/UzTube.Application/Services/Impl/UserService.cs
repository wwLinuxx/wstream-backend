using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UzTube.Application.Exceptions;
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
        string salt = passwordHelper.GenerateSalt();
        string passwordHash = passwordHelper.Encrypt(model.Password, salt);

        User newUser = new User
        {
            Email = model.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            Profile = new Profile
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
        User user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email)
                    ?? throw new BadRequestException("User not found");

        if (!passwordHelper.Verify(user.PasswordHash, model.Password, user.Salt))
            throw new BadRequestException("Email or Password not correct");

        List<string> permissions = await permissionService.GetUserPermissions(user.Id);

        string token = JwtHelper.GenerateToken(user, permissions, configuration);

        return new LoginResponseModel(user.Email, token);
    }

    public async Task<UserResponseModel> GetMeAsync()
    {
        Guid userId = claimService.GetUserId();

        UserResponseModel? user = await context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryId = u.Profile.CountryId,
                CreatedOn = u.CreatedOn.ToString("g"),
                Roles = u.Roles.Select(ur => ur.Role.Name).ToArray()
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<PaginatedList<UserResponseModel>> GetUsersAsync(PageOption option)
    {
        // TODO: When called API GET need hide root user
        
        IQueryable<User> query = context.Users;

        if (!string.IsNullOrEmpty(option.Search))
            query = query.Where(u => u.Email.Contains(option.Search.Trim(), StringComparison.OrdinalIgnoreCase));

        List<UserResponseModel> users = await query
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
                CountryId = u.Profile.CountryId,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .ToListAsync();

        if (users.Count == 0)
            throw new NotFoundException("Users not found");

        int pagesCount = await context.Posts.CountAsync();

        return PaginatedList<UserResponseModel>.Create(users, pagesCount, option.PageNumber, option.PageSize);
    }

    public async Task<UserResponseModel> GetUserProfileByIdAsync(Guid id)
    {
        UserResponseModel? user = await context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryId = u.Profile.CountryId,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UserResponseModel> SearchUserByQueryAsync(string query)
    {
        if (!string.IsNullOrEmpty(query))
            query = query.Trim();

        UserResponseModel? user = await context.Users
            .Include(u => u.Profile)
            .Where(u => u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryId = u.Profile.CountryId,
                CreatedOn = u.CreatedOn.ToString("g")
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UpdateUserProfileResponseModel> UpdateUserProfileByIdAsync(Guid id, UpdateUserProfileModel model)
    {
        User? user = await context.Users
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
        User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("User not found");

        if (!passwordHelper.Verify(user.PasswordHash, model.OldPassword, user.Salt))
            throw new BadRequestException("Old Password not correct");

        if (model.NewPassword != model.ConfirmPassword)
            throw new BadRequestException("NewPassword and ConfirmPassword not equal");

        string newSalt = passwordHelper.GenerateSalt();
        string newHashedPassword = passwordHelper.Encrypt(model.NewPassword, newSalt);

        user.Salt = newSalt;
        user.PasswordHash = newHashedPassword;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserPasswordResponseModel { Id = id };
    }

    public async Task<UpdateUserRoleResponseModel> UpdateUserRoleByIdAsync(Guid id, UpdateUserRoleModel model)
    {
        User user = await context.Users
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
        // TODO: When delete user copy User and user's any posts to HistoryTable
        User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("User not found");

        user.IsDeleted = true;
        user.DeletedOn = DateTime.Now;

        await context.SaveChangesAsync();

        return new DeleteUserResponseModel("Success");
    }

    public async Task<RestoreUserResponseModel> RestoreUserByIdAsync(Guid id)
    {
        // TODO: When restore user copy user and user's any posts from HistoryTable
        User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("User not found");

        user.IsDeleted = false;
        user.DeletedOn = null;

        await context.SaveChangesAsync();

        return new RestoreUserResponseModel { Id = id };
    }
}