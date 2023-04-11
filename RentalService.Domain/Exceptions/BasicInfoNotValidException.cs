namespace RentalService.Domain.Exceptions;

public class BasicInfoNotValidException : NotValidException
{
    internal BasicInfoNotValidException(){}
    internal BasicInfoNotValidException(string message) : base(message) {}
    internal BasicInfoNotValidException(string message, Exception exception) : base(message, exception) {}
}