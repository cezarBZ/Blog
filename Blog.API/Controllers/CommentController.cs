using Blog.Application.Commands.CommentCommands.CreateComment;
using Blog.Application.Commands.CommentCommands.UpdateComment;
using Blog.Application.Commands.DeleteComment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateCommentCommand command)
        {
            command.CommentId = Id;

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var command = new DeleteCommentCommand { CommentId = Id };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
