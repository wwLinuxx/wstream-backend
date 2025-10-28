namespace UzTube.Application.Models.Validators.User;

public static class UserValidatorConfiguration
{
    public const int MinimumEmailLength = 5;
    public const int MaximumEmailLength = 100;

    public const int MinimumPasswordLength = 6;
    public const int MaximumPasswordLength = 128;

    public const int MinimumFirstNameLength = 4;
    public const int MaximumFirstNameLength = 30;

    public const int MinimumLastNameLength = 4;
    public const int MaximumLastNameLength = 30;

    public const int MinimumPhoneNumberLength = 13;
    public const int MaximumPhoneNumberLength = 13;

    public const int MinimumAge = 7;
    public const int MaximumAge = 90;
}