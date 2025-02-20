using Blog.Application.Commands.PostCommands.UpdatePost;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Blog.Tests.Unit.Application.Commands.PostCommands
{
    public class UpdatePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly UpdatePostCommandHandler _handler;

        public UpdatePostCommandHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _handler = new UpdatePostCommandHandler(_postRepositoryMock.Object, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdatePost_WhenPostIsUpdated()
        {
            var command = new UpdatePostCommand
            {
                Id = 1,
                Title = "Title",
                Content = "Content",
                CoverImage = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "CoverImage", "CoverImage.jpg")
            };

            var post = new Post("Title", "Content", "https://www.example.com/image", 1);

            _postRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(post);
            _fileStorageServiceMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("https://www.example.com/image.jpg");
            _postRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Post updated successfuly", result.Message);
        }

        [Fact]
        public async Task Handle_PostNotFound_ShouldReturnNotFound()
        {
            var command = new UpdatePostCommand
            {
                Id = 1,
                Title = "Title",
                Content = "Content",
                CoverImage = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "CoverImage", "CoverImage.jpg")
            };

            _postRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(default(Post));

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Post not found", result.Message);
        }
    }
}
