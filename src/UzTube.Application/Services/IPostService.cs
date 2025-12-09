using Microsoft.AspNetCore.Http;
using UzTube.Application.Models;
using UzTube.Application.Models.Post;

namespace UzTube.Application.Services;

public interface IPostService
{
    Task<UploadVideoFileResponseModel> UploadVideoFileAsync(IFormFile file);
    Task StreamVideoFileAsync(string folder, string file, HttpResponse response);
    Task<CreatePostResponseModel> CreatePostAsync(CreatePostModel request);
    Task<PostResponseModel> GetPostAsync(Guid id);
    Task<PaginatedList<PostResponseModel>> GetPostsAsync(PageOption option);
    Task<PostResponseModel> SearchPostAsync(string query);
    Task<List<PostResponseModel>> GetUserPostsAsync(Guid userId, PageOption option);
    Task<UpdatePostResponseModel> UpdatePostAsync(Guid id, UpdatePostRequest request);
    Task<DeletePostResponseModel> DeletePostAsync(Guid id);
    Task<RestorePostResponseModel> RestorePostAsync(Guid userId);
}