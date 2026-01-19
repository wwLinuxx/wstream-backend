using UzTube.Application.Models;
using UzTube.Application.Models.LiveStream;

namespace UzTube.Application.Services;

public interface IStreamService
{
    Task<StreamResponseModel> CreateStreamAsync(CreateStreamRequest request);
    Task<StreamResponseModel> GetStreamAsync(Guid id);
    Task<PaginatedList<StreamResponseModel>> GetStreamsAsync(PageOption option);
    Task<StreamKeyResponseModel> GetStreamKeyAsync(Guid id);

    // Webhook
    Task<bool> SetOnlineAsync(string name);
    Task<bool> SetOfflineAsync(string name);
}
