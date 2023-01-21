using MediatR;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfile>
{
    
    private readonly DataContext _ctx;
    
    public CreateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<UserProfile> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var basicInfo = UserBasicInfo.CreateUserBasicInfo(request.FirstName, request.LastName, request.DateOfBirth,
            request.CityId, request.Address, request.Contacts);
        
        var profile = UserProfile.CreateUserProfile(basicInfo,Guid.NewGuid().ToString());

        _ctx.Add(profile);
        await _ctx.SaveChangesAsync(cancellationToken);
        return profile;
    }
}