namespace RentalService.Domain.Exceptions;

public class ItemNotValidException : NotValidException
{
    internal ItemNotValidException(){}
    internal ItemNotValidException(string message) : base(message) {}
    internal ItemNotValidException(string message, Exception exception) : base(message, exception) {}
}