namespace Poster.Logic.Common.Validators;

public static class CommentValidator
{
    public static bool IsValidCommentBody(string text)
    {
        return !string.IsNullOrWhiteSpace(text)
               && text.Length <= 256;
    }
    
    public static bool IsValidId(int id)
    {
        return id >= 0;
    }
}