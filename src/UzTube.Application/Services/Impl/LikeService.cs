using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models.Like;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class LikeService(
    DatabaseContext context,
    IClaimService claimService
) : ILikeService
{
    public async Task<CreateLikeResponseModel> LikeAsync(Guid postId)
    {
        Guid userId = claimService.GetUserId();

        if (await context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId))
            throw new BadRequestException("Post already liked");

        Like newLike = new Like
        {
            UserId = userId,
            PostId = postId
        };

        await context.Likes.AddAsync(newLike);
        await context.SaveChangesAsync();

        return new CreateLikeResponseModel("Success");
    }

    public async Task<LikeResponseModel> LikeStatusAsync(Guid postId)
    {
        Guid userId = claimService.GetUserId();

        if (await context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId))
            return new LikeResponseModel { IsLiked = true };
        else
            return new LikeResponseModel { IsLiked = false };
    }

    public async Task<DeleteLikeResponseModel> UnLikeAsync(Guid postId)
    {
        Guid userId = claimService.GetUserId();

        Like like = await context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId)
            ?? throw new BadRequestException("Post already unliked");

        context.Likes.Remove(like);
        await context.SaveChangesAsync();

        return new DeleteLikeResponseModel("Success");
    }
}
