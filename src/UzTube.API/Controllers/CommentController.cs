using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.Application.Services;
using UzTube.Models;

namespace UzTube.API.Controllers
{
    public class CommentController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("get-comments-list")]
        public async Task<IActionResult> GetCommentsList(PageOption option)
        {
            return Ok(ApiResult<PaginatedList<CommentListResponseModel>>.Success(
                await _commentService.GetCommentsList(option)));
        }
    }
}
