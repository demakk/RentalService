namespace RentalService.Api.Registers.App;

public class MvcWebAppRegister : IWebApplicationRegister
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}