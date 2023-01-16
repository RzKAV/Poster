using Poster.Logic.Services.Comments.Dtos;

namespace Poster.Logic.Services.Comments;

public interface ICommentService
{
    Task<List<GetCommentDto>> GetComments();
    
    Task<List<GetCommentDto>> GetCommentsByPost(int postId);
    
    Task<int> CreateComment(CreateCommentDto createCommentDto);
    
    Task EditComment(EditCommentDto editCommentDto);
    
    Task DeleteComment(DeleteCommentDto deleteCommentDto);
}