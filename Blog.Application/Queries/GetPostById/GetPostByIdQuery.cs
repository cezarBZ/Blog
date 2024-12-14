using Blog.Application.Responses;
using Blog.Application.ViewModels;
using MediatR;

namespace Blog.Application.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<Response<PostViewModel>>
    {
        public GetPostByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
