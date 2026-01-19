using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models.Like;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class LikeService(
    DatabaseContext db,
    IClaimService claimService
) : ILikeService
{
    private readonly Guid _currentUserId = claimService.GetUserId();

    public async Task<LikeResponseModel> LikeAsync(Guid postId)
    {
        await ValidatePostAsync(postId);

        if (await IsLikedAsync(postId))
            throw new BadRequestException("Video already liked");

        Like like = new Like
        {
            UserId = _currentUserId,
            PostId = postId
        };

        db.Likes.Add(like);
        await db.SaveChangesAsync();

        return new LikeResponseModel { IsLiked = true };
    }

    public async Task<LikeResponseModel> UnLikeAsync(Guid postId)
    {
        await ValidatePostAsync(postId);

        Like like = await db.Likes
            .FirstOrDefaultAsync(l => l.UserId == _currentUserId && l.PostId == postId)
            ?? throw new BadRequestException("Video already unliked");

        db.Likes.Remove(like);
        await db.SaveChangesAsync();

        return new LikeResponseModel { IsLiked = false };
    }

    public async Task<LikeResponseModel> LikeStatusAsync(Guid postId)
    {
        await ValidatePostAsync(postId);

        bool isLiked = await IsLikedAsync(postId);

        return new LikeResponseModel { IsLiked = isLiked };
    }

    // ===================== Private Methods =====================

    private async Task ValidatePostAsync(Guid postId)
    {
        if (!await db.Posts.AnyAsync(p => p.Id == postId))
            throw new NotFoundException("Post not found");
    }

    private Task<bool> IsLikedAsync(Guid postId) =>
        db.Likes.AnyAsync(l => l.UserId == _currentUserId && l.PostId == postId);
}