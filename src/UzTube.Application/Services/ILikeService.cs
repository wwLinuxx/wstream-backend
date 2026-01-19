using UzTube.Application.Models.Like;

namespace UzTube.Application.Services;

public interface ILikeService
{
    Task<LikeResponseModel> LikeAsync(Guid postId);
    Task<LikeResponseModel> LikeStatusAsync(Guid postId);
    Task<LikeResponseModel> UnLikeAsync(Guid postId);
}
