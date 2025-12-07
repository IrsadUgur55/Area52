using System.Data;
using Microsoft.Data.SqlClient;

namespace Area52.Web.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
