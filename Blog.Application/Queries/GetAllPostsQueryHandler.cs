using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Queries
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IReadOnlyList<PostViewModel>>
    {
        private readonly IPostRepository repository;
        public GetAllPostsQueryHandler(IPostRepository _repository)
        {
            repository = _repository;
        }
        public async Task<IReadOnlyList<PostViewModel>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await repository.GetAllAsync();

            var result = posts.Select(p => new PostViewModel(p.Title, p.Content, p.CoverImageUrl, p.CreatedAt, p.UpdatedAt)).ToList();

            return result;
        }
    }
}
