namespace UzTube.Application.Models.Validators.User;

public static class UserValidatorConfiguration
{
    public const int MinimumEmailLength = 6;
    public const int MaximumEmailLength = 100;

    public const int MinimumUsernameLength = 6;
    public const int MaximumUsernameLength = 20;

    public const int MinimumPasswordLength = 6;
    public const int MaximumPasswordLength = 128;

    public const int MinimumPasswordBigLetterLength = 1;
    public const int MinimumPasswordSmallLetterLength = 1;
    public const int MinimumPasswordNumberLength = 1;
    public const int MinimumPasswordCharacterLength = 1;

    public const string EmailAddressRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
}