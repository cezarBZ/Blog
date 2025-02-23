﻿using Blog.Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Application.Commands.CommentCommands.UpdateComment
{
    public class UpdateCommentCommand : IRequest<Response<Unit>>
    {
        [JsonIgnore]
        public int CommentId { get; set; }
        public string Content { get; set; }
    }
}
