using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.Identity.Commands;

public class RegisterIdentity : IRequest<OperationResult<string>>
{

    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }
    public int CityId { get;  set; }
    public string Address { get;  set; }
    public UserContact[] Contacts { get; set; }
}