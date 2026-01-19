using UzTube.Application.Models.Subscribe;

namespace UzTube.Application.Services;

public interface ISubscribeService
{
    Task<SubscribeResponseModel> SubscribeAsync(Guid userId);
    Task<SubscribeResponseModel> SubscribeStatusAsync(Guid userId);
    Task<SubscribeResponseModel> UnSubscribeAsync(Guid userId);
}
