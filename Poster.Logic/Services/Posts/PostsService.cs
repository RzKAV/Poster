using Microsoft.EntityFrameworkCore;
using Poster.Domain.Entities;
using Poster.Logic.Common.Exceptions.Api;
using Poster.Logic.Common.UserAccessor;
using Poster.Logic.Common.Validators;
using Poster.Logic.Services.Posts.Dtos;

namespace Poster.Logic.Services.Posts;

public class PostsService : IPostsService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserAccessor _userAccessor;

    public PostsService(AppDbContext dbContext, IUserAccessor userAccessor)
    {
        _dbContext = dbContext;
        _userAccessor = userAccessor;
    }

    public async Task<List<GetPostDto>> GetPosts()
    {
        return await _dbContext.Posts.Select(post => new GetPostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Text = post.Text,
            CreationDate = post.CreationDate,
            EditDate = post.EditDate
        }).ToListAsync();
    }

    public async Task<GetPostDto?> GetPostById(int postId)
    {
        if (!PostValidator.IsValidId(postId)) throw new CustomException();

        var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

        if (post == null) throw new CustomException();

        return new GetPostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Text = post.Text,
            CreationDate = post.CreationDate,
            EditDate = post.EditDate
        };
    }

    public async Task<List<GetPostDto>?> GetPostsByUserId()
    {
        return await _dbContext.Posts
            .Where(post => post.UserId == _userAccessor.UserId)
            .Select(post => new GetPostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Text = post.Text,
                CreationDate = post.CreationDate,
                EditDate = post.EditDate
            }).ToListAsync();
    }

    public async Task<int> CreatePost(CreatePostDto postDto)
    {
        if (!PostValidator.IsValidPostBody(postDto.Text)) throw new CustomException();

        var post = new Post
        {
            UserId = _userAccessor.UserId,
            Text = postDto.Text,
            CreationDate = DateTime.Now
        };

        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        return post.Id;
    }

    public async Task EditPost(EditPostDto editPostDto)
    {
        if (!PostValidator.IsValidId(editPostDto.PostId)) throw new CustomException();

        if (!PostValidator.IsValidPostBody(editPostDto.Text)) throw new CustomException();

        var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == editPostDto.PostId);

        if (post == null || post.UserId != _userAccessor.UserId)
            throw new CustomException(errors: "Post not found or this is not your post :)");

        post.Text = editPostDto.Text;
        post.EditDate = DateTime.Now;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePost(DeletePostDto deletePostDto)
    {
        if (!PostValidator.IsValidId(deletePostDto.PostId)) throw new CustomException();

        var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == deletePostDto.PostId);

        if (post == null || post.UserId != _userAccessor.UserId) throw new CustomException();

        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }
}