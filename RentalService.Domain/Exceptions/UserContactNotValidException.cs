namespace RentalService.Domain.Exceptions;

public class UserContactNotValidException : NotValidException
{
    internal UserContactNotValidException(){}
    internal UserContactNotValidException(string message) : base(message) {}
    internal UserContactNotValidException(string message, Exception exception) : base(message, exception) {}
}