using MediatR;
using RentalService.Application.Identity.Commands;
using RentalService.Application.UserProfiles.Commands;

namespace RentalService.Api.Registers.Builder;

public class AddMediatrRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(typeof(RegisterIdentity));
    }
}