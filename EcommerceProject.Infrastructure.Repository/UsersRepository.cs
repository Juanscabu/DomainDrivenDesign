using Dapper;
using EcommerceProject.Domain.Entity;
using EcommerceProject.Infrastructure.Interface;
using EcommerceProject.Transversal.Common;
using System.Data;

namespace EcommerceProject.Infrastructure.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UsersRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public User Authenticate(string username, string password)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "UsersGetByUserAndPassword";
                var parameters = new DynamicParameters();
                parameters.Add("Username", username);
                parameters.Add("Password", password);

                var user = connection.QuerySingle<User>(query,param: parameters, commandType: CommandType.StoredProcedure);
                return user;
            }
    }
}
}
