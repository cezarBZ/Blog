namespace Blog.Application.Responses
{
    public class LoginUserResponse
    {
        public LoginUserResponse(string email, string token)
        {
            Email = email;
            Token = token;
        }
        public string Email { get; private set; }
        public string Token { get; private set; }
    }
}
