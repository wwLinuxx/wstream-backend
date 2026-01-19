using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UzTube.Application.Common.Stream;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.LiveStream;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;
using Stream = UzTube.Core.Entities.Stream;

namespace UzTube.Application.Services.Impl;

public class StreamService(
    DatabaseContext db,
    IClaimService claimService,
    IFileStorageService fileStorageService,
    IOptions<StreamSettings> streamSettings
) : IStreamService
{
    private readonly StreamSettings _streamSettings = streamSettings.Value;

    public async Task<StreamResponseModel> CreateStreamAsync(CreateStreamRequest request)
    {
        Guid userId = claimService.GetUserId();

        string? previewUrl = null;

        if (request.PreviewFile != null)
            previewUrl = await fileStorageService.UploadPreviewFileAsync(request.PreviewFile);

        string streamKey = Guid.NewGuid().ToString("N")[..16];

        Stream newLiveStream = new Stream
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            PreviewUrl = previewUrl,
            StreamKey = streamKey,
            IsPrivate = request.IsPrivate
        };

        await db.Streams.AddAsync(newLiveStream);
        await db.SaveChangesAsync();

        return await GetStreamAsync(newLiveStream.Id);
    }

    public async Task<StreamResponseModel> GetStreamAsync(Guid id)
    {
        StreamResponseModel stream = await db.Streams
            .Where(s => s.Id == id)
            .Include(s => s.User)
            .Select(s => new StreamResponseModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                PreviewUrl = s.PreviewUrl,
                StreamUrl = $"{_streamSettings.HlsServer}/{s.StreamKey}/index.m3u8",
                //StreamUrl = string.IsNullOrEmpty(_streamSettings.HlsPort)
                    //$"{_streamSettings.HlsServer}/{s.StreamKey}/index.m3u8",
                    //? $"{_streamSettings.HlsServer}/{s.StreamKey}/index.m3u8"
                    //: $"{_streamSettings.HlsServer}:{_streamSettings.HlsPort}/{s.StreamKey}/index.m3u8",
                IsLive = s.IsLive,
                StartedAt = s.StartedAt,
                ViewerCount = s.ViewerCount,
                UserId = s.User.Id,
                Username = s.User.Username,
                UserAvatarUrl = s.User.AvatarUrl,
            })
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Stream not found");

        return stream;
    }

    public async Task<PaginatedList<StreamResponseModel>> GetStreamsAsync(PageOption option)
    {
        IQueryable<Stream> query = db.Streams
            .Where(p => !p.IsPrivate && p.IsLive);

        if (!string.IsNullOrWhiteSpace(option.Search))
        {
            string search = option.Search.Trim();
            query = query.Where(p =>
                EF.Functions.ILike(p.Title, $"%{search}%"));
        }

        int streamsCount = await query.CountAsync();

        if (streamsCount == 0)
            return PaginatedList<StreamResponseModel>.Empty(option.PageNumber, option.PageSize);

        List<StreamResponseModel> streams = await query
            .OrderByDescending(s => s.ViewerCount)
                .ThenByDescending(s => s.StartedAt)
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(s => new StreamResponseModel
            {
                Id = s.Id,
                Title = s.Title,
                PreviewUrl = s.PreviewUrl,
                StreamUrl = string.IsNullOrEmpty(_streamSettings.HlsPort)
                    ? $"{_streamSettings.HlsServer}/{s.StreamKey}/index.m3u8"
                    : $"{_streamSettings.HlsServer}:{_streamSettings.HlsPort}/{s.StreamKey}/index.m3u8",
                IsLive = s.IsLive,
                StartedAt = s.StartedAt,
                ViewerCount = s.ViewerCount,
                UserId = s.User.Id,
                Username = s.User.Username,
                UserAvatarUrl = s.User.AvatarUrl,
            })
            .ToListAsync();

        return PaginatedList<StreamResponseModel>.Create(
            streams,
            streamsCount,
            option.PageNumber,
            option.PageSize
        );
    }

    public async Task<StreamKeyResponseModel> GetStreamKeyAsync(Guid streamId)
    {
        Guid userId = claimService.GetUserId();

        Stream stream = await db.Streams
            .FirstOrDefaultAsync(s => s.Id == streamId && s.UserId == userId)
            ?? throw new NotFoundException("Stream not found");

        StreamKeyResponseModel streamKey = new StreamKeyResponseModel
        {
            StreamId = stream.Id,
            RmtpServer = $"{_streamSettings.RtmpServer}:{_streamSettings.RtmpPort}",
            StreamKey = stream.StreamKey
        };

        return streamKey;
    }

    // ===================== Webhook =====================
    public async Task<bool> SetOnlineAsync(string streamKey)
    {
        Stream? stream = await db.Streams
            .FirstOrDefaultAsync(s => s.StreamKey == streamKey)
            ?? throw new NotFoundException($"StreamKey not found");

        stream.IsLive = true;
        stream.StartedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SetOfflineAsync(string streamKey)
    {
        Stream? stream = await db.Streams
            .FirstOrDefaultAsync(s => s.StreamKey == streamKey)
            ?? throw new NotFoundException("StreamKey not found");

        stream.IsLive = false;
        stream.EndedOn = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return true;
    }
}
