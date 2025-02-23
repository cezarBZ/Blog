using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowedQuery : IRequest<Response<IReadOnlyList<UserResponse>>>
    {
        public GetFollowedQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
