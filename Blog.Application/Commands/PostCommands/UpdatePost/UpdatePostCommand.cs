using Blog.Application.Responses;
using Blog.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Blog.Application.Commands.PostCommands.UpdatePost
{
    public class UpdatePostCommand : IRequest<Response<PostViewModel>>
    {
        public UpdatePostCommand()
        {

        }
        public UpdatePostCommand(int id, string title, string content, IFormFile coverImage)
        {
            Id = id;
            Title = title;
            Content = content;
            CoverImage = coverImage;
        }

        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile CoverImage { get; set; }
    }
}
