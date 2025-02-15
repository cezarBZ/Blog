using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;


namespace Blog.Application.Queries.PostQueries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Response<PostResponse>>
    {
        private readonly IPostRepository repository;
        public GetPostByIdQueryHandler(IPostRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Response<PostResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await repository.GetByIdAsync(request.Id);

            if (post == null)
            {
                return Response<PostResponse>.NotFound("Post not found");
            }

            var comments = post.Comments.Select(c => new CommentsResponse { Id = c.Id, Content = c.Content, CreatedAt = c.CreatedAt, UpdatedAt = c.UpdatedAt, UserId = c.UserId }).ToList();

            var postViewModel = new PostResponse { Id = post.Id, Title = post.Title, Content = post.Content, CoverImageUrl = post.CoverImageUrl, CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt, Comments = comments };

            return Response<PostResponse>.Success(postViewModel);
        }
    }
}
