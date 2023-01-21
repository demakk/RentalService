using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.QueryHandlers;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        var profile = await _ctx.UserProfiles
            .Include(up => up.UserBasicInfo)
            .ThenInclude(bi => bi.UserContacts)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
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
        
        result.Payload = profile;
        return result;
    }
}