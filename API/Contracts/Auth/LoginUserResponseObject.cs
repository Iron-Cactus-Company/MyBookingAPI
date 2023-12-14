namespace API.Contracts.Auth;

public class LoginUserResponseObject
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
}