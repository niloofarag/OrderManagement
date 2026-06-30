namespace OrderManagement.Application.Users;

public interface IAuthService
{
    Task<string> LoginAsync(string phoneNumber, string password, CancellationToken cancellationToken = default);
}
