using System.Text.RegularExpressions;

namespace Poster.Logic.Common.Validators;

public static class UserNameValidator
{
    public static bool IsValidUserName(string name)
    {
        return !string.IsNullOrWhiteSpace(name)
               && name.Length >= 5
               && name.Length <= 20
               && Regex.IsMatch(name, "^([a-zA-Z]|[0-9])+$");
    }
}