using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.Application.Services;

namespace UzTube.API.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpPost("get-comments-list")]
    public async Task<IActionResult> GetComments(PageOption option)
    {
        return Ok(ApiResult<PaginatedList<CommentResponseModel>>.Success(
            await commentService.GetCommentsAsync(option)));
    }
}