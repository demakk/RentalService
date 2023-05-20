namespace RentalService.Domain.Exceptions;

public class CustomerNotValidException : NotValidException
{
        internal CustomerNotValidException(){}
        internal CustomerNotValidException(string message) : base(message) {}
        internal CustomerNotValidException(string message, Exception exception) : base(message, exception) {}
}