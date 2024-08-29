using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostResponse>
{
    private readonly IPostRepository repository;
    public CreatePostCommandHandler(IPostRepository _repository)
    {
        repository = _repository;

    }
    public Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var newPost = new Post(request.Title, request.Content);
            repository.Add(newPost);
            var result  = repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            var response = new CreatePostResponse
            {
                Success = true,
                Message = "Post created successfuly",
                id = newPost.Id
            };
            return Task.FromResult(response);
        }
        catch (Exception error)
        {

            var response = new CreatePostResponse
            {
                Success = false,
                Message = $"Error creating post: {error.Message}"
            };
            return Task.FromResult(response);
        }

    }
}
