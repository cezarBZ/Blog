using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Application.Queries.PostQueries.GetAllPosts
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, Response<IReadOnlyList<PostResponse>>>
    {
        private readonly IPostRepository repository;
        public GetAllPostsQueryHandler(IPostRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Response<IReadOnlyList<PostResponse>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await repository.GetAllAsync();

            if (posts.IsNullOrEmpty())
            {
                return Response<IReadOnlyList<PostResponse>>.NotFound("No posts found");
            }

            var result = posts.Select(p => new PostResponse { Id = p.Id, Title = p.Title, Content = p.Content, CoverImageUrl = p.CoverImageUrl, CreatedAt = p.CreatedAt, UpdatedAt = p.UpdatedAt }).ToList();

            return Response<IReadOnlyList<PostResponse>>.Success(result);
        }
    }
}
