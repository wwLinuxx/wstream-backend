using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models.Subscribe;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class SubscribeService(
    IClaimService claimService,
    DatabaseContext db
) : ISubscribeService
{
    private readonly Guid _currentUserId = claimService.GetUserId();

    public async Task<SubscribeResponseModel> SubscribeAsync(Guid userId)
    {
        await ValidateUserAsync(userId);

        if (await IsSubscribedAsync(userId))
            throw new BadRequestException("Already subscribed to this user");

        Follower follower = new Follower
        {
            FollowerId = _currentUserId,
            FollowingId = userId
        };

        db.Followers.Add(follower);
        await db.SaveChangesAsync();

        return new SubscribeResponseModel { IsSubscribed = true };
    }

    public async Task<SubscribeResponseModel> UnSubscribeAsync(Guid userId)
    {
        await ValidateUserAsync(userId);

        Follower follower = await db.Followers
            .FirstOrDefaultAsync(f => f.FollowerId == _currentUserId && f.FollowingId == userId)
            ?? throw new BadRequestException("Not subscribed to this user");

        db.Followers.Remove(follower);
        await db.SaveChangesAsync();

        return new SubscribeResponseModel { IsSubscribed = false };
    }

    public async Task<SubscribeResponseModel> SubscribeStatusAsync(Guid userId)
    {
        await ValidateUserAsync(userId);

        bool isSubscribed = await IsSubscribedAsync(userId);

        return new SubscribeResponseModel { IsSubscribed = isSubscribed };
    }

    // ===================== Private Methods =====================

    private async Task ValidateUserAsync(Guid userId)
    {
        if (userId == _currentUserId)
            throw new BadRequestException("Cannot subscribe to yourself");

        if (!await db.Users.AnyAsync(u => u.Id == userId))
            throw new NotFoundException("User not found");
    }

    private Task<bool> IsSubscribedAsync(Guid userId) =>
        db.Followers.AnyAsync(f => f.FollowerId == _currentUserId && f.FollowingId == userId);
}