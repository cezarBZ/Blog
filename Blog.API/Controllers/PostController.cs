﻿using Blog.Application.Commands.CreateComment;
using Blog.Application.Commands.CreatePost;
using Blog.Application.Commands.DeletePost;
using Blog.Application.Commands.UpdatePost;
using Blog.Application.Queries.GetAllPosts;
using Blog.Application.Queries.GetPostById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {

        var cmd = await _mediator.Send(new GetAllPostsQuery());

        if (!cmd.IsFound)
        {
            return NotFound();
        }

        return Ok(cmd);
    }

    [HttpGet]
    [Route("{Id:int}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var post = await _mediator.Send(new GetPostByIdQuery(Id));

        if (!post.IsFound)
        {
            return NotFound();
        }

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] CreatePostCommand command)
    {

        var cmd = await _mediator.Send(command);

        if (!cmd.IsSuccess)
        {

            return BadRequest(cmd);
        }

        return Ok(cmd);
    }

    [HttpPut("{Id:int}")]
    public async Task<IActionResult> Update(int Id, [FromForm] UpdatePostCommand command)
    {
        command.Id = Id;
        var cmd = await _mediator.Send(command);

        if (!cmd.IsSuccess)
        {

            return BadRequest(cmd);
        }

        return Ok(cmd);
    }

    [HttpDelete]
    [Route("{Id:int}")]
    public async Task<IActionResult> Delete(int Id)
    {

        var cmd = await _mediator.Send(new DeletePostCommand(Id));

        if (!cmd.IsSuccess)
        {

            return NotFound(cmd);
        }
        return Ok(cmd);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentCommand command)
    {
        command.IdPost = id;
        await _mediator.Send(command);

        return NoContent();
    }
}
