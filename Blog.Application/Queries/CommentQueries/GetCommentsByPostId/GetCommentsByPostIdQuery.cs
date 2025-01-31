using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Queries.CommentQueries.GetCommentsByPostId
{
    public class GetCommentsByPostIdQuery : IRequest<Response<PostCommentsResponse>>
    {
        public int PostId { get; set; }
    }
}
