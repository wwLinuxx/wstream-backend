using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;
using UzTube.Application.Common.Email;
using UzTube.Application.Common.Otp;
using UzTube.Application.Exceptions;
using UzTube.Application.Helpers;
using UzTube.Application.Models.OtpCode;
using UzTube.Core.Entities;
using UzTube.Core.Enums;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Services.Impl;

public class OtpEmailService(
    DatabaseContext context,
    IEmailService emailService,
    IOptions<OtpSettings> otpSettings,
    IOptions<SmtpSettings> smtpSettings
) : IOtpService
{
    private readonly OtpSettings _otpSettings = otpSettings.Value;
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

    public async Task<OtpResponseModel> SendOtpAsync(Guid userId)
    {
        User? user = await context.Users
            .Include(u => u.OtpCodes)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundException("User not found.");

        string code = OtpCodeGeneratorHelper.OtpCodeGenerator.ToString();

        OtpCode otpCode = new OtpCode
        {
            UserId = user.Id,
            Code = code
        };

        user.OtpCodes.Add(otpCode);

        bool isSent = await emailService.SendEmailAsync(
            user.Email,
            "UzTube Verification Code",
            OtpEmailBody(code));

        if (!isSent)
            throw new Exception("Failed to send OTP email.");

        await context.SaveChangesAsync();

        return new OtpResponseModel { Id = userId };
    }

    public async Task<OtpResponseModel> VerifyOtpAsync(Guid userId, string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new BadRequestException("OTP code is empty.");

        User? user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundException("User not found.");

        OtpCode? lastOtpCode = await context.OtpCodes
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.GeneratedOn)
            .FirstOrDefaultAsync();

        if (lastOtpCode == null)
            throw new NotFoundException("OTP code is not correct.");

        bool isExpired = lastOtpCode.GeneratedOn.AddMinutes(_otpSettings.ExpireMinutes) < DateTime.UtcNow;

        if (isExpired)
        {
            if (_otpSettings.RemoveAfterVerify)
            {
                context.OtpCodes.Remove(lastOtpCode);
                await context.SaveChangesAsync();
            }

            throw new BadRequestException("OTP code is expired.");
        }

        if (!string.Equals(lastOtpCode.Code?.Trim(), code.Trim(), StringComparison.Ordinal))
            throw new BadRequestException("OTP code is not correct.");

        user.Status = UserStatus.Verified;

        if (_otpSettings.RemoveAfterVerify)
            context.OtpCodes.Remove(lastOtpCode);

        await context.SaveChangesAsync();

        return new OtpResponseModel { Id = userId };
    }

    private string OtpEmailBody(string code)
    {
        StringBuilder builder = new StringBuilder();

        builder.Clear();
        builder.AppendLine("<!DOCTYPE html>");
        builder.AppendLine("<html lang=\"uz\">");
        builder.AppendLine("<head>");
        builder.AppendLine("    <meta charset=\"UTF-8\">");
        builder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        builder.AppendLine($"    <title>{_smtpSettings.SenderName} OTP Tasdiqlash</title>");
        builder.AppendLine("    <style>");
        builder.AppendLine("        * { margin: 0; padding: 0; box-sizing: border-box; }");
        builder.AppendLine("        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #111; color: #fff; }");
        builder.AppendLine("        .email-container { max-width: 600px; margin: 40px auto; background-color: #1a1a1a; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.5); }");
        builder.AppendLine("        .header { background-color: #c0392b; color: #fff; padding: 40px 30px; text-align: center; }");
        builder.AppendLine("        .header h1 { font-size: 28px; font-weight: 700; }");
        builder.AppendLine("        .content { padding: 40px 30px; }");
        builder.AppendLine("        .welcome-text { font-size: 18px; color: #fff; margin-bottom: 15px; text-align:center; }");
        builder.AppendLine("        .description { font-size: 16px; color: #ddd; line-height: 1.7; margin-bottom: 30px; text-align:center; }");
        builder.AppendLine("        .otp-section { text-align: center; margin: 35px 0; }");
        builder.AppendLine("        .otp-code { font-size: 32px; font-weight: bold; color: #c0392b; background: #fff; padding: 20px 30px; border-radius: 12px; letter-spacing: 8px; border: 2px solid #c0392b; cursor: pointer; transition: 0.3s; display: inline-block; }");
        builder.AppendLine("        .otp-code:hover { background: #f2f2f2; }");
        builder.AppendLine("        .otp-label { font-size: 14px; color: #bbb; margin-bottom: 10px; text-transform: uppercase; letter-spacing: 1px; }");
        builder.AppendLine("        .footer { background-color: #111; padding: 25px 30px; text-align: center; font-size: 12px; color: #888; border-top: 1px solid #333; }");
        builder.AppendLine("        .footer .company-name { font-weight: 600; color: #fff; }");
        builder.AppendLine("        @media (max-width: 600px) { .content { padding: 30px 20px; } .header { padding: 30px 20px; } .otp-code { font-size: 28px; letter-spacing: 6px; } }");
        builder.AppendLine("    </style>");
        builder.AppendLine("</head>");
        builder.AppendLine("<body>");
        builder.AppendLine("    <div class=\"email-container\">");
        builder.AppendLine("        <div class=\"header\">");
        builder.AppendLine($"            <h1>{_smtpSettings.SenderName}</h1>");
        builder.AppendLine("        </div>");
        builder.AppendLine("        <div class=\"content\">");
        builder.AppendLine($"            <div class=\"welcome-text\">{_smtpSettings.SenderName} ga xush kelibsiz!</div>");
        builder.AppendLine($"            <div class=\"description\">Hisobingizni tasdiqlash uchun quyidagi <strong style='color:#c0392b'>OTP</strong> koddan foydalaning. Kod {_otpSettings.ExpireMinutes} daqiqa amal qiladi.</div>");
        builder.AppendLine("            <div class=\"otp-section\">");
        builder.AppendLine("                <div class=\"otp-label\">Sizning tasdiqlash kodingiz</div>");
        builder.AppendLine($"                <div class=\"otp-code\" onclick=\"copyCode(this)\" id=\"otpCode\">{code}</div>");
        builder.AppendLine("                <small style='display:block; margin-top:8px; color:#bbb;'>Kod ustiga bosib nusxa oling</small>");
        builder.AppendLine("            </div>");
        builder.AppendLine("        </div>");
        builder.AppendLine("        <div class=\"footer\">");
        builder.AppendLine($"            <p>Bu xabar <span class=\"company-name\">{_smtpSettings.SenderName}</span> tomonidan avtomatik yuborilgan. Javob bermang.</p>");
        builder.AppendLine($"            <p>&copy; 2025 {_smtpSettings.SenderName}. Barcha huquqlar himoyalangan.</p>");
        builder.AppendLine("        </div>");
        builder.AppendLine("    </div>");
        builder.AppendLine("    <script>");
        builder.AppendLine("        function copyCode(element) {");
        builder.AppendLine("            navigator.clipboard.writeText(element.textContent);");
        builder.AppendLine("            element.textContent = 'Nusxa olindi!';");
        builder.AppendLine("            setTimeout(() => element.textContent = '" + code + "', 1500);");
        builder.AppendLine("        }");
        builder.AppendLine("    </script>");
        builder.AppendLine("</body>");
        builder.AppendLine("</html>");


        return builder.ToString();
    }
}