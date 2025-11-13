namespace UzTube.Application.Helpers;

public static class OtpCodeGeneratorHelper
{
    public static int OtpCodeGenerator => new Random().Next(10000, 99999);
}