using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Interfaces;

public interface IPostService
{
    Task<ApiResult> CreatePostAsync(PostCreateDTO dto);
    Task<Result<List<PostGetDTO>>> GetAllPostsAsync();
    Task<Result<PostGetDTO>> GetPostByIdAsync(int id);
    Task<Result<PostGetDTO>> SearchPostByQueryAsync(int id);
    Task<Result<List<PostGetDTO>>> GetUserPostsAsync(int id);
    Task<ApiResult> UpdatePostByIdAsync(int id, PostUpdateDTO dto);
    Task<ApiResult> DeletePostByIdAsync(int id);
    Task<ApiResult> RestorePostByIdAsync(int id);
}