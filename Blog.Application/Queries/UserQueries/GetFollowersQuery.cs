using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowersQuery : IRequest<Response<IReadOnlyList<UserResponse>>>
    {
        public GetFollowersQuery(int userId)
        {
            this.userId = userId;
        }

        public int userId { get; set; }
    }
}
