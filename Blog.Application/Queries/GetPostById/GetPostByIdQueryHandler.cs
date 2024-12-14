using Blog.Application.Responses;
using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;


namespace Blog.Application.Queries.GetPostById
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
            var postViewModel = new PostViewModel(post.Title, post.Content, post.CoverImageUrl, post.CreatedAt, post.UpdatedAt);

            return Response<PostViewModel>.Success(postViewModel);
        }
    }
}
