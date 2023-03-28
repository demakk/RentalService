using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Identity.Commands;

public class LoginCommand : IRequest<GenericOperationResult<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}