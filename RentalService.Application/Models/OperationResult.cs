using RentalService.Application.Enums;

namespace RentalService.Application.Models;

public class OperationResult
{
    public bool IsError { get; private set; }
    public List<Error> Errors { get; set; } = new();

    /// <summary>
    /// Adding error to the list of errors
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public void AddError(ErrorCode code, string message)
    {
        HandleError(code, message);
    }

    /// <summary>
    /// Adding Unknown error to the list of errors
    /// </summary>
    /// <param name="message"></param>
    public void AddUnknownError(string message)
    {
        HandleError(ErrorCode.NotFound, message);
    }

    public void AddValidationError(string message)
    {
        HandleError(ErrorCode.ValidationError, message);
    }


    private void HandleError(ErrorCode code, string message)
    {
        Errors.Add(new Error {Code = code, Message = message});
        IsError = true;
    }
}