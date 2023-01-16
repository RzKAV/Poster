namespace Poster.Logic.Common.Exceptions.Base;

public class BaseException : Exception
{
    public BaseException(string message) : base(message)
    {
    }

    public BaseException()
    {
    }
}