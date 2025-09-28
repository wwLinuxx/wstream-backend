using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IPostRepository
{
    Result CreatePost(PostCreateDTO dto);
    Result<List<PostGetDTO>> GetAllPosts();
    Result<PostGetDTO> GetPostById(int id);
    Result<List<PostGetDTO>> GetUserOwnPosts();
    Result UpdatePostById(PostUpdateDTO dto);
}