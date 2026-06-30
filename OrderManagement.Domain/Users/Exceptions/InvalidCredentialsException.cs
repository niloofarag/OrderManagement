using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Users.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException() : base("Invalid phone number or password")
    {
    }
}
