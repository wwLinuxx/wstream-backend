using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class CommentService(
    DatabaseContext context,
    IClaimService claimService
) : ICommentService
{
    public async Task<CreateCommentResponseModel> CreateCommentAsync(Guid postId, CreateCommentRequest request)
    {
        if (!await context.Posts.AnyAsync(p => p.Id == postId))
            throw new NotFoundException("Video not found");

        Guid userId = claimService.GetUserId();

        Comment newComment = new Comment
        {
            PostId = postId,
            CommentText = request.CommentText,
            UserId = userId
        };

        await context.Comments.AddAsync(newComment);
        await context.SaveChangesAsync();

        return new CreateCommentResponseModel { Id = newComment.Id };
    }

    public async Task<CommentResponseModel> GetCommentAsync(Guid commentId)
    {
        CommentResponseModel? comment = await context.Comments
            .Where(c => c.Id == commentId)
            .Select(c => new CommentResponseModel
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                CommentText = c.CommentText,
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();

        return comment ?? throw new NotFoundException("Comment not found");
    }

    public async Task<PaginatedList<CommentResponseModel>> GetCommentsAsync(Guid postId, PageOption option)
    {
        if (!await context.Posts.AnyAsync(p => p.Id == postId))
            throw new NotFoundException("Video not found");

        IQueryable<Comment> query = context.Comments;

        if (!string.IsNullOrWhiteSpace(option.Search))
        {
            string search = option.Search.Trim();

            query = query.Where(p =>
                EF.Functions.ILike(p.CommentText, $"%{search}%")); // PostgreSQL case-insensitive
        }

        int commentsCount = await query.CountAsync();

        if (commentsCount == 0)
            throw new NotFoundException("Comments not found");

        List<CommentResponseModel> comments = await query
            .Where(p => p.PostId == postId)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(c => new CommentResponseModel
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                CommentText = c.CommentText,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return PaginatedList<CommentResponseModel>.Create(
            comments,
            commentsCount,
            option.PageNumber,
            option.PageSize
        );
    }
}