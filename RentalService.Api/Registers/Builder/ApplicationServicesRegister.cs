using RentalService.Application.Services;

namespace RentalService.Api.Registers.Builder;

public class ApplicationServicesRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();
    }
}