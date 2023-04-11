namespace RentalService.Api.Registers.Builder;

public class AutoMapperRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program));
    }
}