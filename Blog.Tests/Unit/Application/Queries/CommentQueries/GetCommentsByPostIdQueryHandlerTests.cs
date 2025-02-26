using Blog.Application.Queries.CommentQueries.GetCommentsByPostId;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;
using System.Reflection;

namespace Blog.Tests.Unit.Application.Queries.CommentQueries
{
    public class GetCommentsByPostIdQueryHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository;

        public GetCommentsByPostIdQueryHandlerTests()
        {
            _commentRepository = new Mock<ICommentRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnComments_WhenCommentsExist()
        {
            // Arrange
            var user1 = new User(1, "User1", "user1@example.com", "hash", true, UserRole.User, "url");
            var user2 = new User(2, "User2", "user2@example.com", "hash", true, UserRole.User, "url");
            var postId = 1;
            var comment1 = new Comment("Content 1", postId, 1);
            var comment2 = new Comment("Content 2", postId, 2);
            comment1.SetUser(user1);
            comment2.SetUser(user2);
            var comments = new List<Comment>
            {
                comment1, comment2
            };

            var mockCommentRepository = new Mock<ICommentRepository>();
            mockCommentRepository
                .Setup(repo => repo.GetPostComments(postId))
                .ReturnsAsync(comments);

            var handler = new GetCommentsByPostIdQueryHandler(mockCommentRepository.Object);
            var query = new GetCommentsByPostIdQuery(postId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(postId, result.Data.PostId);
            Assert.Equal(comments.Count, result.Data.TotalComments);
            Assert.Equal(comments.Count, result.Data.Comments.Count);

            mockCommentRepository.Verify(repo => repo.GetPostComments(postId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCommentsDoNotExist()
        {
            // Arrange
            var postId = 1;
            var comments = new List<Comment>();

            var mockCommentRepository = new Mock<ICommentRepository>();
            mockCommentRepository
                .Setup(repo => repo.GetPostComments(postId))
                .ReturnsAsync(comments);

            var handler = new GetCommentsByPostIdQueryHandler(mockCommentRepository.Object);
            var query = new GetCommentsByPostIdQuery(postId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Comments not found.", result.Message);

            mockCommentRepository.Verify(repo => repo.GetPostComments(postId), Times.Once);
        }

    }

    public static class CommentExtensions
    {
        public static void SetUser(this Comment comment, User user)
        {
            var property = typeof(Comment).GetProperty("User", BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                property.SetValue(comment, user);
            }
        }
    }
}
