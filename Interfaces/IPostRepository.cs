using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IPostRepository
{
    Result CreatePost(PostCreateDTO dto);
    Result<List<PostViewDTO>> ViewAllPosts();
}