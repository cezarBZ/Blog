using Blog.Application.Commands.CreatePost;
using Blog.Application.Commands.DeletePost;
using Blog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

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
    public async Task<IActionResult> GetAll()
    {

        var cmd = await _mediator.Send(new GetAllPostsQuery());

        return Ok(cmd);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] CreatePostCommand command)
    {

        var cmd = await _mediator.Send(command);

        if (cmd.Success)
        {

            return Ok(cmd);
        }
        return BadRequest(cmd);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletePostCommand command)
    {

        var cmd = await _mediator.Send(command);

        if (cmd.Success)
        {

            return Ok(cmd);
        }
        return BadRequest(cmd);
    }
}
