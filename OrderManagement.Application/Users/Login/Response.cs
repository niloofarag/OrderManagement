namespace OrderManagement.Application.Users.Login;

internal sealed record Response
{
    public string Token { get; init; } = default!;
}
