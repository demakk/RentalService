using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfile>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<UserProfile> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _ctx.UserProfiles
            .Include(up => up.UserBasicInfo)
            .ThenInclude(bi => bi.UserContacts)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
        
        var basicInfo = UserBasicInfo.CreateUserBasicInfo(request.FirstName, request.LastName, request.DateOfBirth,
            request.CityId, request.Address, request.Contacts);


        
        profile.UserBasicInfo.UpdateBasicInfo(basicInfo);
        
        profile.UpdateUserProfile();
        
        _ctx.UserProfiles.Update(profile);
        
        await _ctx.SaveChangesAsync(cancellationToken);

        return profile;
    }
}