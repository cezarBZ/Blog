using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<CreatePostResponse>
    {
        public CreatePostCommand(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}

//aplicar validations