using UzTube.Application.Models;
using UzTube.Application.Models.Post;
using UzTube.Models.DTO;

namespace UzTube.Interfaces;

public interface IPostService
{
    Task<CreatePostResponseModel> CreatePostAsync(CreatePostModel dto, Guid id);

    Task<List<PostResponseModel>> GetPostsAsync();
    
    Task<PaginationResult<PostListResonseModel>> GetListPostsAsync(PageOption option);

    Task<PostResponseModel> GetPostByIdAsync(Guid id);

    Task<PostResponseModel> SearchPostByQueryAsync(Guid id);

    Task<List<PostResponseModel>> GetUserPostsAsync(Guid id);
    
    Task<UpdatePostResponseModel> UpdatePostByIdAsync(Guid id, UpdatePostModel dto);
    
    Task<DeletePostResponseModel> DeletePostByIdAsync(Guid id);

    Task<RestorePostResponseModel> RestorePostByIdAsync(Guid id);
}