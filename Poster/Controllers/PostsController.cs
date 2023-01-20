using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poster.Logic.Services.Posts;
using Poster.Logic.Services.Posts.Dtos;

namespace Poster.Controllers;

[Route("api/posts")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostsService _postsService;

    public PostsController(IPostsService postsService)
    {
        _postsService = postsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var result = await _postsService.GetPosts();

        return Ok(result);
    }

    [HttpGet("{postId:int}")]
    public async Task<IActionResult> GetPostById([FromRoute] int postId)
    {
        var result = await _postsService.GetPostById(postId);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetPostsByUserId()
    {
        var result = await _postsService.GetPostsByUserId();

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto createPostDto)
    {
        var result = await _postsService.CreatePost(createPostDto);

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditPost([FromBody] EditPostDto editPostDto)
    {
        await _postsService.EditPost(editPostDto);

        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletePostDto deletePostDto)
    {
        await _postsService.DeletePost(deletePostDto);

        return Ok();
    }
}