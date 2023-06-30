using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EcommerceProject.Persistence.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NorthwindConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
