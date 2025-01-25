using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Commands.UserCommands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
