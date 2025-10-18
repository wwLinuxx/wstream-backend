using UzTube.Application.Models;
using UzTube.Application.Models.Comment;

namespace UzTube.Application.Services;

public interface ICommentService
{
    Task<PaginatedList<CommentListResponseModel>> GetCommentsList(PageOption option);
}
