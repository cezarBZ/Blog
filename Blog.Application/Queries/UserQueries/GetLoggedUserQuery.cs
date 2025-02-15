using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetLoggedUserQuery : IRequest<Response<UserResponse>>
    {
    }
}
