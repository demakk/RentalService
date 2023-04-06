using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RentalService.Dal;

public class DapperContext 
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DapperString");
    }

    public IDbConnection Connect()
    {
        return new SqlConnection(_connectionString);
    }
}