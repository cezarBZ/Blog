using MediatR;
using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.LikeAggregate;

namespace Blog.Application.Queries.LikeQueries.GetLikesByPostId
{
    public class GetLikesByPostIdQueryHandler : IRequestHandler<GetLikesByPostIdQuery, Response<PostLikesResponse>>
    {
        private readonly ILikeRepository _likeRepository;

        public GetLikesByPostIdQueryHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<Response<PostLikesResponse>> Handle(GetLikesByPostIdQuery request, CancellationToken cancellationToken)
        {
            var likes = await _likeRepository.GetPostLikes(request.PostId);
            var likesCount = likes.Count;

            if (likesCount < 1)
            {
                return Response<PostLikesResponse>.Failure("Likes not found.");
            }

            var response = new PostLikesResponse
            {
                PostId = request.PostId,
                TotalLikes = likes.Count,
                Likes = likes.Select(l => new PostLikeResponse
                {
                    LikeId = l.Id,
                    UserId = l.UserId,
                    UserName = l.User.Username,
                    LikedAt = l.LikedAt
                }).ToList()
            };

            return Response<PostLikesResponse>.Success(response);
        }
    }
}
