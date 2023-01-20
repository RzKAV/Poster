using Microsoft.EntityFrameworkCore;
using Poster.Domain.Entities;
using Poster.Logic.Common.Exceptions.Api;
using Poster.Logic.Common.UserAccessor;
using Poster.Logic.Common.Validators;
using Poster.Logic.Services.Comments.Dtos;

namespace Poster.Logic.Services.Comments;

public class CommentService : ICommentService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserAccessor _userAccessor;

    public CommentService(AppDbContext dbContext, IUserAccessor userAccessor)
    {
        _dbContext = dbContext;
        _userAccessor = userAccessor;
    }

    public async Task<List<GetCommentDto>> GetComments()
    {
        return await _dbContext.Comments.Select(comment => new GetCommentDto
        {
            Id = comment.Id,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Text = comment.Text,
            CreationDate = comment.CreationDate,
            EditDate = comment.EditDate
        }).ToListAsync();
    }

    public async Task<List<GetCommentDto>> GetCommentsByPost(int postId)
    {
        if (!CommentValidator.IsValidId(postId))
        {
            throw new CustomException("invalid post id");
        }

        return await _dbContext.Comments
            .Where(comment => comment.PostId == postId)
            .Select(comment => new GetCommentDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Text = comment.Text,
                CreationDate = comment.CreationDate,
                EditDate = comment.EditDate
            }).ToListAsync();
    }

    public async Task<int> CreateComment(CreateCommentDto createCommentDto)
    {
        if (!CommentValidator.IsValidCommentBody(createCommentDto.Text))
        {
            throw new CustomException("invalid post body");
        }

        var postId = createCommentDto.PostId;
        var isExistPost = await _dbContext.Posts.AnyAsync(p => p.Id == postId);

        if (!isExistPost)
        {
            throw new CustomException("post not found");
        }

        var comment = new Comment
        {
            PostId = postId,
            UserId = _userAccessor.UserId,
            Text = createCommentDto.Text,
            CreationDate = DateTime.Now
        };

        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();

        return comment.Id;
    }

    public async Task EditComment(EditCommentDto editCommentDto)
    {
        if (!(CommentValidator.IsValidCommentBody(editCommentDto.Text)
              && CommentValidator.IsValidId(editCommentDto.Id)))
        {
            throw new CustomException("invalid comment id or body");
        }

        var comment =
            await _dbContext.Comments.FirstOrDefaultAsync(
                comment => comment.Id == editCommentDto.Id);

        if (comment == null || _userAccessor.UserId != comment.UserId)
        {
            throw new CustomException("comment not found");
        }

        comment.Text = editCommentDto.Text;
        comment.EditDate = DateTime.Now;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteComment(DeleteCommentDto deleteCommentDto)
    {
        if (!CommentValidator.IsValidId(deleteCommentDto.CommentId))
        {
            throw new CustomException("invalid comment id");
        }

        var comment =
            await _dbContext.Comments.FirstOrDefaultAsync(comment =>
                comment.Id == deleteCommentDto.CommentId);

        if (comment == null || _userAccessor.UserId != comment.UserId)
        {
            throw new CustomException("comment not found or this is not your comment");
        }

        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }
}