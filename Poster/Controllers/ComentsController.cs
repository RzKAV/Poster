using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poster.Logic.Services.Comments;
using Poster.Logic.Services.Comments.Dtos;

namespace Poster.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetComments()
    {
        var result = await _commentService.GetComments();

        return Ok(result);
    }

    [HttpGet("postId/{postId:int}")]
    public async Task<IActionResult> GetCommentsByPostId(int postId)
    {
        var result = await _commentService.GetCommentsByPost(postId);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
    {
        var result = await _commentService.CreateComment(createCommentDto);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<IActionResult> EditComment(EditCommentDto editCommentDto)
    {
        await _commentService.EditComment(editCommentDto);

        return Ok();
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteComment(DeleteCommentDto deleteCommentDto)
    {
        await _commentService.DeleteComment(deleteCommentDto);

        return Ok();
    }
}