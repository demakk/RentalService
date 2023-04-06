namespace RentalService.Domain.Exceptions;

public class ShoppingCartNotValidException : NotValidException
{
    internal ShoppingCartNotValidException(){}
    internal ShoppingCartNotValidException(string message) : base(message) {}
    internal ShoppingCartNotValidException(string message, Exception exception) : base(message, exception) {}
}