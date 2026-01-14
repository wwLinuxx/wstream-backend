using UzTube.Application.Models.View;

namespace UzTube.Application.Services;

public interface IViewService
{
    Task<ViewResponseModel> ViewPostAsync(Guid postId);
}
