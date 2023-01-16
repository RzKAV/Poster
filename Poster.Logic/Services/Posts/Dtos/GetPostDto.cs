using Poster.Domain.Entities;

namespace Poster.Logic.Services.Posts.Dtos;

public class GetPostDto
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public string Text { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? EditDate { get; set; }
}