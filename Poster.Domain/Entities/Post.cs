namespace Poster.Domain.Entities;

public class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; }

    public string Text { get; set; }

    public List<Comment> Comments { get; set; }
    
    public DateTime CreationDate { get; set; }

    public DateTime? EditDate { get; set; }
}