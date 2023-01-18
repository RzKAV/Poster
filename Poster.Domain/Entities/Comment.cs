namespace Poster.Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }

    public string Text { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? EditDate { get; set; }
}