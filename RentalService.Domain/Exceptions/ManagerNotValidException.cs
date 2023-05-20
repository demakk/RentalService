namespace RentalService.Domain.Exceptions;

public class ManagerNotValidException : NotValidException
{
    internal ManagerNotValidException(){}
    internal ManagerNotValidException(string message) : base(message) {}
    internal ManagerNotValidException(string message, Exception exception) : base(message, exception) {}
}