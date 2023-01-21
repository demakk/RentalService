using Microsoft.EntityFrameworkCore;
using RentalService.Dal;

namespace RentalService.Api.Registers.Builder;

public class DbRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(cs);});
    }
}