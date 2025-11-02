using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.Application.Services;

namespace UzTube.API.Controllers;

public class CommentController(ICommentService commentService) : ApiController
{
    [HttpPost("get-comments-list")]
    public async Task<IActionResult> GetCommentsList(PageOption option)
    {
        return Ok(ApiResult<PaginatedList<CommentResponseModel>>.Success(
            await commentService.GetCommentsList(option)));
    }
}