using UzTube.Application.Models;
using UzTube.Application.Models.Comment;

namespace UzTube.Application.Services;

public interface ICommentService
{
    Task<PaginatedList<CommentResponseModel>> GetCommentsList(PageOption option);
}