using Poster.Logic.Services.Posts.Dtos;

namespace Poster.Logic.Services.Posts;

public interface IPostsService
{
    public Task<List<GetPostDto>> GetPosts();

    public Task<GetPostDto> GetPostById(int postId);

    public Task<List<GetPostDto>> GetPostsByUserId();

    public Task<int> CreatePost(CreatePostDto createPostDto);

    public Task EditPost(EditPostDto editPostDto);

    public Task DeletePost(DeletePostDto deletePostDto);
}