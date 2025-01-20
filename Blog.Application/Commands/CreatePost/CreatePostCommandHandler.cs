using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Response<int>>
{
    private readonly IPostRepository repository;
    private readonly IFileStorageService fileStorageService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CreatePostCommandHandler(IPostRepository _repository, IFileStorageService _fileStorageService, IHttpContextAccessor _httpContextAccessor)
    {
        repository = _repository;
        fileStorageService = _fileStorageService;
        httpContextAccessor = _httpContextAccessor;

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
            var userIdClaim = httpContextAccessor.HttpContext.User.Claims
                                    .FirstOrDefault(c => c.Type == "userId");
            int userId = int.Parse(userIdClaim.Value);

            var newPost = new Post(request.Title, request.Content, coverImageUrl, userId);
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
