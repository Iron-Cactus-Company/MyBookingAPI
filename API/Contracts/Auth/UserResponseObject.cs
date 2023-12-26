namespace API.Contracts.Auth;

public class UserResponseObject
{
    public string Id { get; init; }
    public string Email { get; init; }

    public Domain.Company Company { get; init; }
}