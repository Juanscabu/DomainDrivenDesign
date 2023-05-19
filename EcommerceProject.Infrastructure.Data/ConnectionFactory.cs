using System;
using EcommerceProject.Transversal.Common;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace EcommerceProject.Infrastructure.Data
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public IDbConnection? GetConnection
        { 
            get
            {
                var sqlConnection = new SqlConnection();
                if (sqlConnection == null)
                    return null;
                sqlConnection.ConnectionString = _configuration.GetConnectionString("NorthWindConnection");
                sqlConnection.Open();
                return sqlConnection;
            }
        }
    }
}