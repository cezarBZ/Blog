using Blog.Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Application.Commands.UpdateComment
{
    public class UpdateCommentCommand : IRequest<Response<Unit>>
    {
        [JsonIgnore]

        public int PostId { get; set; }
        [JsonIgnore]
        public int CommentId { get; set; }
        public string Content { get; set; }
    }
}
