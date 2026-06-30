using FastEndpoints;

namespace OrderManagement.Application.Users.Login;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IAuthService _authService;

    public Endpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var token = await _authService.LoginAsync(req.PhoneNumber, req.Password, ct);
        await Send.OkAsync(new Response { Token = token }, ct);
    }
}
