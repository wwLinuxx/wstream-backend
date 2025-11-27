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
    IPasswordHasher passwordHasher
) : IUserService
{
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
                CreatedOn = u.CreatedOn,
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
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryId = u.Profile.CountryId,
                CreatedOn = u.CreatedOn
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
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                CountryId = u.Profile.CountryId,
                CreatedOn = u.CreatedOn
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
                CreatedOn = u.CreatedOn
            })
            .FirstOrDefaultAsync();

        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<UpdateUserProfileResponseModel> UpdateUserProfileAsync(Guid id, UpdateUserRequest request)
    {
        User? user = await context.Users
                         .Include(u => u.Profile)
                         .ThenInclude(p => p.Country)
                         .FirstOrDefaultAsync(u => u.Id == id)
                     ?? throw new NotFoundException("User not found");

        user.Profile.FirstName = request.FirstName;
        user.Profile.LastName = request.LastName;
        user.Profile.PhoneNumber = request.PhoneNumber;
        user.Profile.Age = request.Age;
        user.Profile.CountryId = request.CountryId;

        user.UpdatedOn = DateTime.Now;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return new UpdateUserProfileResponseModel { Id = user.Id };
    }

    public async Task<UpdateUserPasswordResponseModel> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordRequest request)
    {
        User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("User not found");

        if (!passwordHasher.Verify(user.PasswordHash, request.OldPassword, user.Salt))
            throw new BadRequestException("Old Password not correct");

        if (request.NewPassword != request.ConfirmPassword)
            throw new BadRequestException("NewPassword and ConfirmPassword not equal");

        string newSalt = passwordHasher.GenerateSalt();
        string newHashedPassword = passwordHasher.Encrypt(request.NewPassword, newSalt);

        user.Salt = newSalt;
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
        user.DeletedOn = DateTime.Now;

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