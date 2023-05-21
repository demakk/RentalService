using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Identity.Commands;

public class RegisterManagerCommand : IRequest<GenericOperationResult<string>>
{
    public string UserProfileId { get; set; }
    public decimal Salary { get; set; }
    public Guid? BossId { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? FireDate { get; set; }   
}