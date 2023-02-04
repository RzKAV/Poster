namespace Poster.Domain.Entities;

public class Token
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; }

    public string Client { get; set; }

    public string Value { get; set; }

    public DateTime ExpireTime { get; set; }

    public DateTime UpdatedTime { get; set; }
}