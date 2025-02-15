using Blog.Application.Commands.LikeCommands.DislikeComment;
using Blog.Application.Commands.LikeCommands.DislikePost;
using Blog.Application.Commands.LikeCommands.LikeComment;
using Blog.Application.Commands.LikeCommands.LikePost;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("like/post/{postId}")]
        public async Task<IActionResult> LikePost(int postId)
        {
            var command = new LikePostCommand { postId = postId };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("dislike/post/{postId}")]
        public async Task<IActionResult> DislikePost(int postId)
        {
            var command = new DislikePostCommand { PostId = postId };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("dislike/comment/{commentId}")]
        public async Task<IActionResult> DislikeComment(int commentId)
        {
            var command = new DislikeCommentCommand { CommentId = commentId };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("like/comment/{commentId}")]
        public async Task<IActionResult> LikeComment(int commentId)
        {
            var command = new LikeCommentCommand { commentId = commentId };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
