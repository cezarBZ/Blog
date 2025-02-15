using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Queries.PostQueries.GetAllPosts
{
    public class GetAllPostsQuery : IRequest<Response<IReadOnlyList<PostResponse>>>
    {
    }
}
