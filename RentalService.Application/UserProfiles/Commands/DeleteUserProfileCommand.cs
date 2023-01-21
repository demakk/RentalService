using MediatR;

namespace RentalService.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest
{
    public Guid Id { get; set; }
}