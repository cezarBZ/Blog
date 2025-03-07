namespace Blog.Application.Responses
{
    public class SignupResponse
    {
        public SignupResponse(string email, string token, string username, string role)
        {
            Email = email;
            Token = token;
            Username = username;
            Role = role;
        }
        public string Email { get; private set; }
        public string Token { get; private set; }
        public string Username { get; private set; }
        public string Role { get; private set; }

    }
}
