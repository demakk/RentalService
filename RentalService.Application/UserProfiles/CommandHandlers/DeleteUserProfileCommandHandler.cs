using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand>
{
    private readonly DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profileToDelete = await _ctx.UserProfiles
            .Include(up => up.UserBasicInfo)
            .ThenInclude(bi => bi.UserContacts)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        _ctx.UserProfiles.Remove(profileToDelete);
        await _ctx.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}