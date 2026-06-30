using OrderManagement.Domain.Users;

namespace OrderManagement.Application.Users;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
