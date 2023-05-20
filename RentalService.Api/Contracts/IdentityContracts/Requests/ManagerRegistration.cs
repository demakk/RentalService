using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.IdentityContracts;

public class ManagerRegistration
{
    public string UserProfileId { get; set; }
    public decimal Salary { get; set; }
    public Guid? BossId { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? FireDate { get; set; }
}