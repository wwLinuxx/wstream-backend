using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Services.Impl;

public class CommentService(DatabaseContext context) : ICommentService
{
    public async Task<PaginatedList<CommentResponseModel>> GetCommentsList(PageOption option)
    {
        IQueryable<Comment> query = context.Comments;

        var comments = await query
            .Skip(option.PageSize * (option.PageNumber - 1))
            .Take(option.PageSize)
            .Select(c => new CommentResponseModel
            {
                Id = c.Id,
                CommentText = c.CommentText,
                CreatedAt = c.CreatedAt.ToString("g")
            })
            .ToListAsync();

        if (comments.Count > 0)
            throw new NotFoundException("Comments not found");

        var commentsCount = context.Comments.Count();

        return PaginatedList<CommentResponseModel>.Create(comments, commentsCount, option.PageNumber, option.PageSize);
    }
}