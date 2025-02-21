﻿using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.UserCommands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserResponse>
    {


        public string Email { get; set; }
        public string Password { get; set; }
    }
}
