namespace UzTube.Application.Exeptions;

public class UnprocessableRequestException(string message) : Exception(message);