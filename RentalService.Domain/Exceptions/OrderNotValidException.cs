namespace RentalService.Domain.Exceptions;

public class OrderNotValidException : NotValidException
{
    internal OrderNotValidException(){}
    internal OrderNotValidException(string message) : base(message) {}
    internal OrderNotValidException(string message, Exception exception) : base(message, exception) {}
}