using Poster.Logic.Common.Exceptions.Base;

namespace Poster.Logic.Common.Exceptions.Api;

public class CustomException : BaseException
{
    public CustomException() : base("Something went wrong")
    {
        Failures = new Dictionary<string, string[]>();
    }
    
    public CustomException(params string[]? errors) : this()
    {
        if (errors != null)
        {
            Failures.Add(String.Empty, errors);
        }
    }

    public IDictionary<string, string[]> Failures { get;}
}