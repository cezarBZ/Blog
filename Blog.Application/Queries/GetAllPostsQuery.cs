using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Queries
{
    public class GetAllPostsQuery : IRequest<IReadOnlyList<PostViewModel>>
    {
    }
}
