using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Queries.PostQueries.GetPostById
{
    public class GetPostByIdQuery : IRequest<Response<PostResponse>>
    {
        public GetPostByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
