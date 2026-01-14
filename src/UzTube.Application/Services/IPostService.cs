using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Application.Models.User;

namespace UzTube.Application.Services;

public interface IPostService
{
    Task<CreatePostResponseModel> CreatePostAsync(CreatePostRequest request);
    Task<PostResponseModel> GetPostAsync(Guid id);
    Task<PaginatedList<PostResponseModel>> GetPostsAsync(PageOption option);
    Task<PostResponseModel> SearchPostAsync(string query);
    Task<List<PostResponseModel>> GetUserPostsAsync(Guid userId, PageOption option);
    Task<UpdatePostResponseModel> UpdatePostAsync(Guid id, UpdatePostRequest request);
    Task<DeletePostResponseModel> DeletePostAsync(Guid id);
    Task<RestorePostResponseModel> RestorePostAsync(Guid userId);
}