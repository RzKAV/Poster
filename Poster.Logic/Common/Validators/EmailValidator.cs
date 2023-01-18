using System.Text.RegularExpressions;

namespace Poster.Logic.Common.Validators;

public static class EmailValidator
{
    private const string EmailRegex =
        @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]{2,}\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email)
               && email.Length <= 100
               && Regex.IsMatch(email, EmailRegex);
    }
}