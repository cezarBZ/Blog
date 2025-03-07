using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Blog.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommand : IRequest<Response<SignupResponse>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public UserRole Role { get; set; }
    }
}
