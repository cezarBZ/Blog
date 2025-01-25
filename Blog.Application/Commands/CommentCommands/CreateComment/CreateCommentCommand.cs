using Blog.Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Application.Commands.CommentCommands.CreateComment;

public class CreateCommentCommand : IRequest<Response<Unit>>
{
    public string Content { get; set; }
    public int postId { get; set; }
}
