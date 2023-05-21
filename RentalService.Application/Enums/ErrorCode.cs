namespace RentalService.Application.Enums;

public enum ErrorCode
{
    NotFound = 404,
    ServerError = 500,
    
    //Validation errors should be in the range 100-199
    ValidationError = 101,
    
    //Infrastructure errors should be in the range 200-299
    IdentityUserAlreadyExists = 201, 
    IdentityCreationFailed = 202,
    PasswordNotValid = 203,
    UnauthorizedAccountRemoval = 204,
    UserDoesNotExist = 205,
        
    UnknownError = 1001
    
}