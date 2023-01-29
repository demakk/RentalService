using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        try
        {
            var profile = await _ctx.UserProfiles
                .Include(up => up.UserBasicInfo)
                .ThenInclude(bi => bi.UserContacts)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

            if (profile is null)
            {
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"User with id {request.Id} not found"
                };

                result.IsError = true;
                result.Errors.Add(error);
                return result;
            }

            var basicInfo = UserBasicInfo.CreateUserBasicInfo(request.FirstName, request.LastName, request.DateOfBirth,
                request.CityId, request.Address, request.Contacts);

            profile.UserBasicInfo.UpdateBasicInfo(basicInfo);

            profile.UpdateUserProfile();

            _ctx.UserProfiles.Update(profile);

            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = profile;
        }
        catch (BasicInfoNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e =>
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = e
                };

                result.Errors.Add(error);
            });
        }
        catch (Exception exception)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = exception.Message
            };
            result.Errors.Add(error);
        }


        return result;
    }
}