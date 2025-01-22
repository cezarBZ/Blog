using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Application.Commands.CreateComment;

public class CreateCommentCommand : IRequest<Unit>
{
    public string Content { get; set; }
    public int postId { get; set; }
}
