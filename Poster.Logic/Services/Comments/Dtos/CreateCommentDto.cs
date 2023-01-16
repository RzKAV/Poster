namespace Poster.Logic.Services.Comments.Dtos;

public class CreateCommentDto
{
    public int PostId { get; set; }

    public string Text { get; set; }
}