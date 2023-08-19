namespace domain.Exceptions;

public class Conflict : ApplicationException
{
    public Conflict(string? message) : base(message)
    {
    }
}