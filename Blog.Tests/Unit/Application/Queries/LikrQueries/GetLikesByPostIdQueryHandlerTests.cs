using Blog.Application.Queries.CommentQueries.GetCommentsByPostId;
using Blog.Application.Queries.LikeQueries.GetLikesByPostId;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;
using System.Reflection;

namespace Blog.Tests.Unit.Application.Queries.LikrQueries
{
    public class GetLikesByPostIdQueryHandlerTests
    {
        private readonly Mock<ILikeRepository> _likeRepository;

        public GetLikesByPostIdQueryHandlerTests()
        {
            _likeRepository = new Mock<ILikeRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnComments_WhenCommentsExist()
        {
            var user1 = new User(1, "User1", "user1@example.com", "hash", true, UserRole.User, "url");
            var postId = 1;
            var like1 = new Like(postId, user1.Id, LikeTargetType.Post);
            var like2 = new Like(postId, user1.Id, LikeTargetType.Post);
            like1.SetUser(user1);
            like2.SetUser(user1);
            var likes = new List<Like>
            {
                like1, like2
            };

            _likeRepository
                .Setup(repo => repo.GetPostLikes(postId))
                .ReturnsAsync(likes);

            var handler = new GetLikesByPostIdQueryHandler(_likeRepository.Object);
            var query = new GetLikesByPostIdQuery(postId);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(postId, result.Data.PostId);
            Assert.Equal(likes.Count, result.Data.TotalLikes);
            Assert.Equal(likes.Count, result.Data.Likes.Count);

            _likeRepository.Verify(repo => repo.GetPostLikes(postId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCommentsDoNotExist()
        {
            var postId = 1;
            var likes = new List<Like>();

            _likeRepository
                .Setup(repo => repo.GetPostLikes(postId))
                .ReturnsAsync(likes);

            var handler = new GetLikesByPostIdQueryHandler(_likeRepository.Object);
            var query = new GetLikesByPostIdQuery(postId);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Likes not found.", result.Message);

            _likeRepository.Verify(repo => repo.GetPostLikes(postId), Times.Once);
        }
    }

    public static class LikeExtensions
    {
        public static void SetUser(this Like like, User user)
        {
            var property = typeof(Like).GetProperty("User", BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                property.SetValue(like, user);
            }
        }
    }
}
