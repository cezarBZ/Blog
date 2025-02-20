using Blog.Application.Commands.PostCommands.CreatePost;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Services.BlobStorage;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Blog.Tests.Unit.Application.Commands.PostCommands
{
    public class CreatePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly CreatePostCommandHandler _handler;

        public CreatePostCommandHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new CreatePostCommandHandler(_postRepositoryMock.Object, _fileStorageServiceMock.Object, _userContextServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreatePost_WhenPostIsCreated()
        {
            var command = new CreatePostCommand
            {
                Title = "Title",
                Content = "Content",
                CoverImage = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "CoverImage", "CoverImage.jpg")
            };

            _fileStorageServiceMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("https://www.example.com/image.jpg");
            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _postRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Post created successfuly", result.Message);
        }
    }
}
