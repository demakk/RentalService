namespace RentalService.Application.Identity;

public static class IdentityErrorMessages
{
    public static string IdentityNotFound = "Provided username does not exist";
    public static string IncorrectPassword = "Provided password is incorrect";
    public static string UserAlreadyExists = "Provided username already exists";
    public static string UnauthorizedAccountRemoval = "Cannot remove account because you are not its creator";
}