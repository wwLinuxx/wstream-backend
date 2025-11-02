namespace UzTube.Application.Models.Validators.Post;

public static class PostValidatorConfiguration
{
    public const int MaximumTitleLength = 100;
    public const int MinimumTitleLength = 10;

    public const int MaximumDescriptionLength = 1000;
    public const int MinimumDescriptionLength = 10;
}