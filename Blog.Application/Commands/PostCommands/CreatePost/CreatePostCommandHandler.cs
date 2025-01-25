using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.PostCommands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Response<int>>
{
    private readonly IPostRepository repository;
    private readonly IFileStorageService fileStorageService;
    private readonly IUserContextService userContextService;

    public CreatePostCommandHandler(IPostRepository _repository, IFileStorageService _fileStorageService, IUserContextService _userContextService)
    {
        repository = _repository;
        fileStorageService = _fileStorageService;
        userContextService = _userContextService;
    }
    public async Task<Response<int>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string coverImageUrl = null;
            if (request.CoverImage != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CoverImage.FileName)}";
                coverImageUrl = await fileStorageService.UploadFileAsync(request.CoverImage, fileName);
            }

            var userId = userContextService.GetUserId();

            var newPost = new Post(request.Title, request.Content, coverImageUrl, userId.Value);
            await repository.AddAsync(newPost);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(newPost.Id, "Post created successfuly");
        }
        catch (Exception error)
        {
            return Response<int>.Failure($"Error creating post: {error.Message}");
        }

    }
}
