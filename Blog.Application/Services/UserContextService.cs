using Blog.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public UserContextService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User> GetUserAsync()
        {
            int? userId = GetUserIdFromClaims();

            if (userId == null)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);
            Console.WriteLine(user);

            return user;
        }

        public int? GetUserId()
        {
            return GetUserIdFromClaims();
        }

        private int? GetUserIdFromClaims()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null)
            {
                return null;
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}
