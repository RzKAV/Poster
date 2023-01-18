using System.ComponentModel;

namespace Poster.Domain.Enums;

public enum Roles
{
    [Description("admin")] Admin = 1,

    [Description("user")] User = 2
}