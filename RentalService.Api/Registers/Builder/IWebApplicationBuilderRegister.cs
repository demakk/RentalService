namespace RentalService.Api.Registers.Builder;

public interface IWebApplicationBuilderRegister : IRegister
{
    public void RegisterServices(WebApplicationBuilder builder);
}