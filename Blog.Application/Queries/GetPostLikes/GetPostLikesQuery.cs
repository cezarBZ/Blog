using MediatR;
using Blog.Application.Responses;

namespace Blog.Application.Queries.GetPostLikes
{
    public class GetPostLikesQuery : IRequest<Response<GetPostLikesViewModel>>
    {
        public int PostId { get; set; }
    }

}

