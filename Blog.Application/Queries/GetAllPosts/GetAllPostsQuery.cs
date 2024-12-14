using Blog.Application.Responses;
using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Queries.GetAllPosts
{
    public class GetAllPostsQuery : IRequest<Response<IReadOnlyList<PostViewModel>>>
    {
    }
}
