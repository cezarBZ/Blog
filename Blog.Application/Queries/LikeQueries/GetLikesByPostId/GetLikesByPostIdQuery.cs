using MediatR;
using Blog.Application.Responses;

namespace Blog.Application.Queries.LikeQueries.GetLikesByPostId
{
    public class GetLikesByPostIdQuery : IRequest<Response<PostLikesResponse>>
    {
        public int PostId { get; set; }
    }

}

