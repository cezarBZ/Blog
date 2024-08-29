using Blog.Application.Commands.CreatePost;
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

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatePostCommand command)
    {

        var cmd = await _mediator.Send(command);
        return Ok(cmd);
    }
}
