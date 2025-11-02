using UzTube.Application.Models;
using UzTube.Application.Models.Post;

namespace UzTube.Application.Services;

public interface IPostService
{
    Task<CreatePostResponseModel> CreatePostAsync(CreatePostModel model);
    Task<PaginatedList<PostResponseModel>> GetPostsAsync(PageOption option);
    Task<PostResponseModel> GetPostByIdAsync(Guid id);
    Task<PostResponseModel> SearchPostByQueryAsync(string query);
    Task<List<PostResponseModel>> GetUserPostsAsync(Guid userId, PageOption option);
    Task<UpdatePostResponseModel> UpdatePostByIdAsync(Guid id, UpdatePostModel model);
    Task<DeletePostResponseModel> DeletePostByIdAsync(Guid id);
    Task<RestorePostResponseModel> RestorePostByIdAsync(Guid userId);
}