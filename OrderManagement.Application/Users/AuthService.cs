using OrderManagement.Domain.Users;
using OrderManagement.Domain.Users.Exceptions;

namespace OrderManagement.Application.Users;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> LoginAsync(string phoneNumber, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindByPhoneNumberAsync(phoneNumber, cancellationToken);
        if (user is null || !_passwordHasher.Verify(password, user.PasswordHash))
            throw new InvalidCredentialsException();

        return _jwtTokenGenerator.GenerateToken(user);
    }
}
