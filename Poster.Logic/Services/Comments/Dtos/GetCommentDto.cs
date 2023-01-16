namespace Poster.Logic.Services.Comments.Dtos;

public class GetCommentDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    public string Text { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? EditDate { get; set; }
}