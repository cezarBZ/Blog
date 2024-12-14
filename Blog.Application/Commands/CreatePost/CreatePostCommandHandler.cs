using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;

namespace Blog.Application.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Response<int>>
{
    private readonly IPostRepository repository;
    private readonly IFileStorageService fileStorageService;
    public CreatePostCommandHandler(IPostRepository _repository, IFileStorageService _fileStorageService)
    {
        repository = _repository;
        fileStorageService = _fileStorageService;

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

            var newPost = new Post(request.Title, request.Content, coverImageUrl);
            repository.Add(newPost);
            var result = repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(newPost.Id, "Post created successfuly");
        }
        catch (Exception error)
        {

            return Response<int>.Failure($"Error creating post: {error.Message}");
        }

    }
}
