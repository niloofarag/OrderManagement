namespace OrderManagement.Application.Users.Login;

internal sealed record Request
{
    public string PhoneNumber { get; init; } = default!;

    public string Password { get; init; } = default!;
}
