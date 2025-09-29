using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IPostRepository
{
    Task<Result> CreatePost(PostCreateDTO dto);
    Task<Result<List<PostGetDTO>>> GetAllPosts();
    Task<Result<PostGetDTO>> GetPostById(int id);
    Task<Result<List<PostGetDTO>>> GetUserPosts(int id);
    Task<Result> UpdatePostById(int id, PostUpdateDTO dto);
}