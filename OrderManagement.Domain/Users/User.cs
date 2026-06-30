using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Users;

public class User : BaseEntity
{
    public string FullName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public UserRole Role { get; set; }
}
