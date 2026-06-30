namespace OrderManagement.Domain.Users;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
}
