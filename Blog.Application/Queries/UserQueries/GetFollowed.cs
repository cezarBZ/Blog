using Blog.Application.Responses;
using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowedQuery : IRequest<Response<IReadOnlyList<UserViewModel>>>
    {
        public GetFollowedQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
