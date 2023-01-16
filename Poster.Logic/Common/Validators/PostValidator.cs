namespace Poster.Logic.Common.Validators;

public static class PostValidator
{
    public static bool IsValidPostBody(string text)
    {
        return !string.IsNullOrWhiteSpace(text)
               && text.Length <= 512;
    }
    
    public static bool IsValidId(int id)
    {
        return id >= 0;
    }
}