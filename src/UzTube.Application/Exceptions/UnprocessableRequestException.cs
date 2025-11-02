namespace UzTube.Application.Exceptions;

public class UnprocessableRequestException(string message) : Exception(message);