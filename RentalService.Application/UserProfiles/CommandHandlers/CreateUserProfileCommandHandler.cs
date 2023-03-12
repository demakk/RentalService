using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, OperationResult<UserProfile>>
{
    
    private readonly DataContext _ctx;
    
    public CreateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        try
        {
            var basicInfo = UserBasicInfo.CreateUserBasicInfo(request.FirstName, request.LastName, request.DateOfBirth,
                request.CityId, request.Address, request.Contacts);

            var profile = UserProfile.CreateUserProfile(basicInfo, Guid.NewGuid().ToString());

            _ctx.UserProfiles.Add(profile);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = profile;
        }
        catch (BasicInfoNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => { result.AddValidationError(e); });
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        return result;
    }
}