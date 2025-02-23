using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.CommentAggregate;
using MediatR;

namespace Blog.Application.Queries.CommentQueries.GetCommentsByPostId
{
    public class GetCommentsByPostIdQueryHandler : IRequestHandler<GetCommentsByPostIdQuery, Response<PostCommentsResponse>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsByPostIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Response<PostCommentsResponse
            >> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetPostComments(request.PostId);
            var commentsCount = comments.Count;

            if (commentsCount < 1)
            {
                return Response<PostCommentsResponse>.Failure("Comments not found.");
            }

            var response = new PostCommentsResponse
            {
                PostId = request.PostId,
                TotalComments = commentsCount,
                Comments = comments.Select(c => new PostCommentResponse
                {
                    CommentId = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.Username,
                    CreatedAt = c.CreatedAt,
                    Content = c.Content,
                    LikeCount = c.LikeCount
                }).ToList()
            };

            return Response<PostCommentsResponse>.Success(response);

        }
    }
}