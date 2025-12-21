namespace UzTube.Application.Models.User;

public record LoginUserRequest(
    string Login,
    string Password);

public record LoginResponseModel(
    string Login,
    string Token);