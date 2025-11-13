using UzTube.Application.Models.OtpCode;

namespace UzTube.Application.Services;

public interface IOtpService
{
    Task<OtpResponseModel> SendOtpAsync(Guid userId);
    Task<OtpResponseModel> VerifyOtpAsync(Guid userId, string code);
}