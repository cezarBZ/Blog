namespace Blog.Domain.Core.Auth;

public interface IAuthService
{
    string GenerateJwtToken(string email, string role, int id);
    string ComputeSha256Hash(string password);
}
