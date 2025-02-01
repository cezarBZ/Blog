using Blog.Application.Responses;
using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetLoggedUserQuery : IRequest<Response<UserViewModel>>
    {
    }
}
