using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exeptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.DataAccess.Persistence;
using UzTube.Entities;

namespace UzTube.Application.Services.Impl;

public class CommentsService : ICommentService
{
    private readonly DatabaseContext _context;

    public CommentsService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CommentListResponseModel>> GetCommentsList(PageOption option)
    {
        IQueryable<PostComment> query = _context.PostComments;

        if (string.IsNullOrWhiteSpace(option.Search))
            query = query.Where(c => c.Comment.Contains(option.Search));

        List<CommentListResponseModel> comments = await query
            .Skip(option.PageSize * (option.PageNumber - 1))
            .Take(option.PageSize)
            .Select(c => new CommentListResponseModel
            {
                Id = c.Id,
                Comment = c.Comment,
                CreatedAt = c.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss")
            })
            .ToListAsync();

        if (comments.Count > 0)
            throw new NotFoundException("Comments not found");

        int commentsCount = _context.PostComments.Count();

        return PaginatedList<CommentListResponseModel>.Create(
            comments,
            commentsCount,
            option.PageNumber,
            option.PageSize);
    }
}
