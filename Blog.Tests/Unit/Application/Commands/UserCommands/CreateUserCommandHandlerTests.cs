﻿using Blog.Application.Commands.UserCommands.CreateUser;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using Blog.Infrastructure.Services.BlobStorage;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Blog.Tests.Unit.Application.Commands.UserCommands
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly Mock<IAuthService> _authServiceMock;

        public CreateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _authServiceMock = new Mock<IAuthService>();
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenUserIsCreated()
        {
            var command = new CreateUserCommand
            {
                Username = "Username",
                Email = "Email",
                Password = "Password",
                Role = UserRole.User,
                ProfilePicture = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "ProfilePicture", "ProfilePicture.jpg")
            };

            _authServiceMock.Setup(x => x.ComputeSha256Hash(It.IsAny<string>())).Returns("PasswordHash");
            _fileStorageServiceMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("https://www.example.com/image.jpg");
            _userRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _authServiceMock.Object, _fileStorageServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Usuário criado com sucesso.", result.Message);
        }
    }
}
