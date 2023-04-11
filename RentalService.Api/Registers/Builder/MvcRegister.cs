using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Filters;

namespace RentalService.Api.Registers.Builder;

public class MvcRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(config =>
        {
            config.Filters.Add(typeof(RentalServiceExceptionHandler));
        });

        builder.Services.AddEndpointsApiExplorer();
    }
}