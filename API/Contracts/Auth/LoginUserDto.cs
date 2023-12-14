namespace API.Contracts.Auth;

public class LoginUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string AccessToken { get; set; }
}