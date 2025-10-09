using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IPostRepository
{
    Task<Result> CreatePostAsync(PostCreateDTO dto);
    Task<Result<List<PostGetDTO>>> GetAllPostsAsync();
    Task<Result<PostGetDTO>> GetPostByIdAsync(int id);
    Task<Result<PostGetDTO>> SearchPostByQueryAsync(int id);
    Task<Result<List<PostGetDTO>>> GetUserPostsAsync(int id);
    Task<Result> UpdatePostByIdAsync(int id, PostUpdateDTO dto);
    Task<Result> DeletePostByIdAsync(int id);
    Task<Result> RestorePostByIdAsync(int id);
}