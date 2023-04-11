using RentalService.Application.Enums;

namespace RentalService.Application.Models;

public class Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
}