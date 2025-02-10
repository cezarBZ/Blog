using Blog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommand : IRequest<Response<int>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public IFormFile ProfilePicture { get; set; }

    }
}
