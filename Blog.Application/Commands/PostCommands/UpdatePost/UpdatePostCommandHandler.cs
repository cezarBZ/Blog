using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;

namespace Blog.Application.Commands.PostCommands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Response<PostResponse>>
    {
        private readonly IPostRepository postRepository;
        private readonly IFileStorageService fileStorageService;
        public UpdatePostCommandHandler(IPostRepository _postRepository, IFileStorageService fileStorageService)
        {
            postRepository = _postRepository;
            this.fileStorageService = fileStorageService;
        }

        public async Task<Response<PostResponse>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            string coverImageUrl = null;
            var post = await postRepository.GetByIdAsync(request.Id);

            if (post == null)
            {
                return Response<PostResponse>.NotFound("Post not found");
            }

            if (request.CoverImage != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CoverImage.FileName)}";
                coverImageUrl = await fileStorageService.UploadFileAsync(request.CoverImage, fileName);
            }

            post.Edit(request.Title, request.Content, coverImageUrl);

            postRepository.Update(post);

            await postRepository.UnitOfWork.SaveChangesAsync();

            var postViewModel = new PostResponse { Title = post.Title, Content = post.Content, CoverImageUrl = post.CoverImageUrl, CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt };

            return Response<PostResponse>.Success(postViewModel, "Post updated successfuly");

        }
    }
}
