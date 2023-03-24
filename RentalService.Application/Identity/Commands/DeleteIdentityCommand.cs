using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Identity.Commands;

public class DeleteIdentityCommand : IRequest<OperationResult<bool>>
{
    public Guid RequesterId { get; set; }
    public Guid IdentityUserId { get; set; }
}