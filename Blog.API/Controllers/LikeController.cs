using Blog.Application.Commands.LikeCommands.Dislike;
using Blog.Application.Commands.LikeCommands.LikeComment;
using Blog.Application.Commands.LikeCommands.LikePost;
using Blog.Application.Queries.GetPostLikes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("post/{postId}")]
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Dislike(int Id)
        {
            var command = new DislikeCommand { Id = Id };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("comment/{commentId}")]
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

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostLikes(int postId)
        {
            var query = new GetPostLikesQuery { PostId = postId };
            var response = await _mediator.Send(query);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
