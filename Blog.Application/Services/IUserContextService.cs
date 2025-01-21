using Blog.Domain.AggregatesModel.UserAggregate;

namespace Blog.Application.Services
{
    public interface IUserContextService
    {
        Task<User> GetUserAsync();
        int? GetUserId();

    }
}
