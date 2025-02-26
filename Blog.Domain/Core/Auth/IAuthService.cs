using Blog.Domain.AggregatesModel.UserAggregate;

namespace Blog.Domain.Core.Auth;

public interface IAuthService
{
    string GenerateJwtToken(string email, UserRole role, int id);
    string ComputeSha256Hash(string password);
}
