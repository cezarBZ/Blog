using Blog.Application.Responses;
using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowersQuery : IRequest<Response<IReadOnlyList<UserViewModel>>>
    {
        public GetFollowersQuery(int userId)
        {
            this.userId = userId;
        }

        public int userId { get; set; }
    }
}
