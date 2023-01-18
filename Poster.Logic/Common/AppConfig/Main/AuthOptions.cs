using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Poster.Logic.Common.AppConfig.Main;

public record AuthOptions
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string SecretKey { get; set; }

    public int ExpireTimeTokenMinutes { get; set; }

    public SymmetricSecurityKey SymmetricSecurityKey
    {
        get
        {
            if (string.IsNullOrWhiteSpace(SecretKey))
            {
                throw new NullReferenceException(nameof(SecretKey));
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        }
    }
};