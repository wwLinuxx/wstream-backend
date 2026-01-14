using UzTube.Application.Models.Like;

namespace UzTube.Application.Services;

public interface ILikeService
{
    Task<CreateLikeResponseModel> LikeAsync(Guid postId);
    Task<LikeResponseModel> LikeStatusAsync(Guid postId);
    Task<DeleteLikeResponseModel> UnLikeAsync(Guid postId);
}
