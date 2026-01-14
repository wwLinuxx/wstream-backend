using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models.View;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class ViewService(
    DatabaseContext context,
    IClaimService claimService
) : IViewService
{
    public async Task<ViewResponseModel> ViewPostAsync(Guid postId)
    {
        Post? post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postId)
            ?? throw new NotFoundException("Video not found");

        Guid userId = claimService.GetUserId();

        post.ViewsCount++;

        if (userId != Guid.Empty)
        {
            bool alreadyViewed = await context.Views
                .AnyAsync(v => v.UserId == userId && v.PostId == postId);

            if (!alreadyViewed)
            {
                await context.Views.AddAsync(new View
                {
                    PostId = postId,
                    UserId = userId
                });
            }
        }
        else
        {
            await context.Views.AddAsync(new View
            {
                PostId = postId
            });
        }

        context.Update(post);
        await context.SaveChangesAsync();

        return new ViewResponseModel("Success");
    }
}
