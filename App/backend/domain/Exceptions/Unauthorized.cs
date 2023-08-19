namespace domain.Exceptions;

public class Unauthorized : ApplicationException
{
    public Unauthorized(string? message) : base(message)
    {
    }
}