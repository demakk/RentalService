using RentalService.Api.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace RentalService.Api.Registers.Builder;

public class SwaggerRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}