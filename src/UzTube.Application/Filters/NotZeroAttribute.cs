using System.ComponentModel.DataAnnotations;

namespace UzTube.Attributes;

public class NotZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = "UserId most be under 0";

        if (value is int number && number > 0)
            return true;
        else if (value is IEnumerable<int> numbers && numbers.All(n => n > 0))
            return true;

        return false;
    }
}
