﻿using MediatR;

namespace Blog.Application.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
