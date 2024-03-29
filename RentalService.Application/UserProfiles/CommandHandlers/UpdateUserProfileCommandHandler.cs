﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, GenericOperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<GenericOperationResult<UserProfile>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<UserProfile>();
        try
        {
            var profile = await _ctx.UserProfiles
                .Include(up => up.UserBasicInfo)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

            if (profile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfileErrorMessages.UserProfileNotFound, request.Id));
                return result;
            }

            var basicInfo = UserBasicInfo.CreateUserBasicInfo(profile.Id, request.FirstName, request.LastName, request.DateOfBirth,
                request.PassportId, request.Phone, request.Email);

            profile.UserBasicInfo.UpdateBasicInfo(basicInfo);

            profile.UpdateUserProfile();

            _ctx.UserProfiles.Update(profile);

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