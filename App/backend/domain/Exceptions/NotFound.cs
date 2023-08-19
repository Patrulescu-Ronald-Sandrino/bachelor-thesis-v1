namespace domain.Exceptions;

public class NotFound : ApplicationException
{
    public NotFound(string? message) : base(message)
    {
    }
}