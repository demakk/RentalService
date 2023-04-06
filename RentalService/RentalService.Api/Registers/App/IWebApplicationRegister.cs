namespace RentalService.Api.Registers.App;

public interface IWebApplicationRegister : IRegister
{
    public void RegisterPipelineComponents(WebApplication app);
}