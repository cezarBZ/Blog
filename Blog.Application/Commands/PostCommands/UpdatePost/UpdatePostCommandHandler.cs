using Blog.Application.Responses;
using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;

namespace Blog.Application.Commands.PostCommands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Response<PostViewModel>>
    {
        private readonly IPostRepository postRepository;
        private readonly IFileStorageService fileStorageService;
        public UpdatePostCommandHandler(IPostRepository _postRepository, IFileStorageService fileStorageService)
        {
            postRepository = _postRepository;
            this.fileStorageService = fileStorageService;
        }

        public async Task<Response<PostViewModel>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            string coverImageUrl = null;
            var post = await postRepository.GetByIdAsync(request.Id);

            if (post == null)
            {
                return Response<PostViewModel>.NotFound();
            }

            if (request.CoverImage != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CoverImage.FileName)}";
                coverImageUrl = await fileStorageService.UploadFileAsync(request.CoverImage, fileName);
            }

            post.Edit(request.Title, request.Content, coverImageUrl);

            postRepository.Update(post);

            await postRepository.UnitOfWork.SaveChangesAsync();

            var postViewModel = new PostViewModel { Title = post.Title, Content = post.Content, CoverImageUrl = post.CoverImageUrl, CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt };

            return Response<PostViewModel>.Success(postViewModel);

        }
    }
}
