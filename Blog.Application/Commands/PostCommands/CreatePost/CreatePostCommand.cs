using Blog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Blog.Application.Commands.PostCommands.CreatePost
{
    public class CreatePostCommand : IRequest<Response<int>>
    {
        public CreatePostCommand() { }
        public CreatePostCommand(string title, string content, IFormFile coverImage)
        {
            Title = title;
            Content = content;
            CoverImage = coverImage;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile CoverImage { get; set; }
    }
}

//aplicar validations