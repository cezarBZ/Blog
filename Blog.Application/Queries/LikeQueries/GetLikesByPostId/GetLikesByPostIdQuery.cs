using MediatR;
using Blog.Application.Responses;

namespace Blog.Application.Queries.LikeQueries.GetLikesByPostId
{
    public class GetLikesByPostIdQuery : IRequest<Response<PostLikesResponse>>
    {

        public GetLikesByPostIdQuery(int postId)
        {
            PostId = postId;
        }
        public int PostId { get; set; }
    }

}

