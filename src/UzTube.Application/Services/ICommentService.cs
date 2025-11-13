using UzTube.Application.Models;
using UzTube.Application.Models.Comment;

namespace UzTube.Application.Services;

public interface ICommentService
{
    Task<CreateCommentResponseModel> CreateCommentAsync(Guid postId, CreateCommentRequest request);
    Task<CommentResponseModel> GetCommentAsync(Guid commentId);
    Task<PaginatedList<CommentResponseModel>> GetCommentsAsync(PageOption option);
}