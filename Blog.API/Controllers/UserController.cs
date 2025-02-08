using Blog.Application.Commands.UserCommands.CreateUser;
using Blog.Application.Commands.UserCommands.Follow;
using Blog.Application.Commands.UserCommands.LoginUser;
using Blog.Application.Commands.UserCommands.Unfollow;
using Blog.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var loginUserViewModel = await _mediator.Send(command);

            if (loginUserViewModel == null)
                return BadRequest();

            return Ok(loginUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoggedUser()
        {
            var query = new GetLoggedUserQuery();
            var response = await _mediator.Send(query);

            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        [HttpPost("/follow/{followedId}")]
        public async Task<IActionResult> FollowUser(int followedId)
        {
            var command = new FollowUserCommand(followedId);

            var result = await _mediator.Send(command);

            if (result == null)
                return BadRequest();

            return Ok(result.Message);
        }

        [HttpPost("/unfollow/{followedId}")]
        public async Task<IActionResult> UnfollowUser(int followedId)
        {
            var command = new UnfollowUserCommand(followedId);
            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest();

            return Ok(result.Message);
        }

        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            var query = new GetFollowersQuery(userId);
            var response = await _mediator.Send(query);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }

    }
}
