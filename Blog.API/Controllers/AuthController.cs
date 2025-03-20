using Blog.Application.Commands.UserCommands.CreateUser;
using Blog.Application.Commands.UserCommands.LoginUser;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("signin")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var loginUserViewModel = await _mediator.Send(command);

            if (loginUserViewModel == null)
                return BadRequest();

            return Ok(loginUserViewModel);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> CreateRegularUser([FromBody] CreateUserCommand command)
        {
            command.Role = UserRole.User;
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);

            }

            return Ok(response);
        }

        [HttpPost("createAdmin")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateAdminUser([FromBody] CreateUserCommand command)
        {
            command.Role = UserRole.Admin;
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);

            }

            return Ok(response.Message);
        }
    }
}
