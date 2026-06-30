using FastEndpoints.Security;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Users;
using OrderManagement.Domain.Users;

namespace OrderManagement.Infrastructure.Users;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var signingKey = _configuration["Jwt:SigningKey"]!;

        return JwtBearer.CreateToken(o =>
        {
            o.SigningKey = signingKey;
            o.ExpireAt = DateTime.UtcNow.AddHours(8);
            o.User.Roles.Add(user.Role.ToString());
            o.User["UserId"] = user.Id.ToString();
            o.User["FullName"] = user.FullName;
        });
    }
}
