using MediatR;
using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.LikeAggregate;

namespace Blog.Application.Queries.GetPostLikes
{
    public class GetPostLikesQueryHandler : IRequestHandler<GetPostLikesQuery, Response<GetPostLikesViewModel>>
    {
        private readonly ILikeRepository _likeRepository;

        public GetPostLikesQueryHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<Response<GetPostLikesViewModel>> Handle(GetPostLikesQuery request, CancellationToken cancellationToken)
        {
            var likes = await _likeRepository.GetPostLikes(request.PostId);

            var response = new GetPostLikesViewModel
            {
                PostId = request.PostId,
                TotalLikes = likes.Count,
                Likes = likes.Select(l => new PostLikeDto
                {
                    UserId = l.UserId,
                    UserName = l.User.Username,
                    LikedAt = l.LikedAt
                }).ToList()
            };

            return Response<GetPostLikesViewModel>.Success(response);
        }
    }
}
