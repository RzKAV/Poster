using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Poster.Logic.Services.Comments.Dtos;
using Poster.Domain.Entities;
using Poster.Logic.Common.Exceptions.Api;
using Poster.Logic.Common.UserAccessor;

namespace Poster.Logic.Services.Comments;

public class CommentService : ICommentService
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserAccessor _userAccessor;

    public CommentService(IAppDbContext dbContext, IUserAccessor userAccessor)
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
        var comment = new Comment
        {
            PostId = createCommentDto.PostId,
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
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == editCommentDto.Id);

        if (comment == null || _userAccessor.UserId != comment.UserId)
        {
            throw new CustomException();
        }

        comment.Text = editCommentDto.Text;
        comment.EditDate = DateTime.Now;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteComment(DeleteCommentDto deleteCommentDto)
    {
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == deleteCommentDto.CommentId);

        if (comment == null || _userAccessor.UserId != comment.UserId)
        {
            throw new CustomException();
        }

        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }
}