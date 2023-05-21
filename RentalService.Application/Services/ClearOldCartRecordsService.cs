using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RentalService.Application.Services;


/*public class ClearOldCartRecordsService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IConfiguration _configuration;


    public ClearOldCartRecordsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(RemoveCardRecords, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void RemoveCardRecords(object state)
    {
        string str = "DELETE FROM ShoppingCarts WHERE ClearDate <= GETDATE()";
        
        using var connection = new SqlConnection(_configuration.GetConnectionString("DapperString"));
        connection.Open();
        Thread.Sleep(60000);
        var result = connection.Execute(str);
        connection.Close();
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}*/