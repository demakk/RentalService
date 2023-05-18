using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.Identity.Commands;

public class RegisterIdentity : IRequest<GenericOperationResult<string>>
{

    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }

    public string? PassportId { get; set; }
    public string Phone { get; set; }
}