using Blog.Application.Responses;
using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;


namespace Blog.Application.Queries.PostQueries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Response<PostViewModel>>
    {
        private readonly IPostRepository repository;
        public GetPostByIdQueryHandler(IPostRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Response<PostViewModel>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await repository.GetByIdAsync(request.Id);

            if (post == null)
            {
                return Response<PostViewModel>.NotFound("Post not found");
            }

            var comments = post.Comments.Select(c => new CommentViewModel { Id = c.Id, Content = c.Content, CreatedAt = c.CreatedAt, UpdatedAt = c.UpdatedAt, UserId = c.UserId }).ToList();

            var postViewModel = new PostViewModel { Id = post.Id, Title = post.Title, Content = post.Content, CoverImageUrl = post.CoverImageUrl, CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt, Comments = comments };

            return Response<PostViewModel>.Success(postViewModel);
        }
    }
}
