using UzTube.Application.Models;

namespace UzTube.Models.DTO;

public class PostResponseModel : BaseResponseModel
{
    public Guid UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string PhotoUrl { get; set; }

    public string VideoUrl { get; set; }

    public string Duration { get; set; }

    public string PostedAt { get; set; }

    public int ViewsCount { get; set; }

    public int LikesCount { get; set; }

    public int Rating { get; set; }

    public bool IsPrivate { get; set; }
}