using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Helpers.Interfaces;
using UzTube.Application.Models;
using UzTube.Application.Models.User;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class UserService(
    DatabaseContext context,
    IClaimService claimService,
    IPasswordHasher passwordHasher,
    IFileStorageService fileStorageService
) : IUserService
{
    public async Task<UserResponseModel> GetProfileAsync()
    {
        Guid userId = claimService.GetUserId();

        UserResponseModel? user = await context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                AvatarFile = u.AvatarUrl,
                CountryId = u.CountryId,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn,
                Roles = u.Roles.Select(ur => ur.Role.Name).ToArray()
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UserResponseModel> GetUserAsync(Guid id)
    {
        UserResponseModel? user = await context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CountryId = u.CountryId,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UserResponseModel> GetUserAsync(string username)
    {
        UserResponseModel? user = await context.Users
            .Where(u => u.Username == username)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                AvatarFile = u.AvatarUrl,
                CountryId = u.CountryId,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UserPreviewResponseModel> GetUserPreviewAsync(Guid id)
    {
        UserPreviewResponseModel? user = await context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserPreviewResponseModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<PaginatedList<UserResponseModel>> GetUsersAsync(PageOption option)
    {
        // TODO: When called API GET need hide root user

        IQueryable<User> query = context.Users;

        if (!string.IsNullOrEmpty(option.Search))
            query = query.Where(u => u.Email.Contains(option.Search.Trim()));

        List<UserResponseModel> users = await query
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CountryId = u.CountryId,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn
            })
            .ToListAsync();

        if (users.Count == 0)
            throw new NotFoundException("Users not found");

        int pagesCount = await context.Posts.CountAsync();

        return PaginatedList<UserResponseModel>.Create(users, pagesCount, option.PageNumber, option.PageSize);
    }

    public async Task<UserResponseModel> SearchUserAsync(string query)
    {
        if (!string.IsNullOrEmpty(query))
            query = query.Trim();

        UserResponseModel? user = await context.Users
            .Where(u => u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CountryId = u.CountryId,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UserResponseModel> UpdateUserProfileAsync(Guid id, UpdateUserRequest request)
    {
        User? user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User not found");


        if (request.AvatarFile != null)
        {
            string? avatarUrl = await fileStorageService.UploadAvatarFileAsync(request.AvatarFile);

            user.AvatarUrl = avatarUrl;
        }

        user.UpdatedOn = DateTime.UtcNow;
        user.Username = request.Username;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return await GetProfileAsync();
    }

    public async Task<UpdateUserPasswordResponseModel> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordRequest request)
    {
        User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User not found");

        if (!passwordHasher.Verify(user.PasswordHash, request.OldPassword))
            throw new BadRequestException("Old Password not correct");

        string newHashedPassword = passwordHasher.Hash(request.NewPassword);

        user.PasswordHash = newHashedPassword;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserPasswordResponseModel { Id = id };
    }

    public async Task<UpdateUserRoleResponseModel> UpdateUserRolesAsync(Guid id, UpdateUserRolesRequest request)
    {
        User user = await context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User not found");

        user.Roles = request.Roles
            .Select(rid => new UserRole
            {
                RoleId = rid
            })
            .ToList();

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserRoleResponseModel { Id = id };
    }

    public async Task<DeleteUserResponseModel> DeleteUserAsync(Guid id)
    {
        // TODO: When delete user copy User and user's any posts to HistoryTable
        User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User not found");

        user.IsDeleted = true;
        user.DeletedOn = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new DeleteUserResponseModel("Success");
    }

    public async Task<RestoreUserResponseModel> RestoreUserAsync(Guid id)
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