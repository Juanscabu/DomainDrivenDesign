﻿using Dapper;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Domain.Entities;
using EcommerceProject.Persistence.Contexts;
using System.Data;

namespace EcommerceProject.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DapperContext _context;

        public UsersRepository(DapperContext context)
        {
            _context = context;
        }
        public User Authenticate(string username, string password)
        {
            using (var connection = _context.CreateConnection())
            {
                var query = "UsersGetByUserAndPassword";
                var parameters = new DynamicParameters();
                parameters.Add("Username", username);
                parameters.Add("Password", password);

                var user = connection.QuerySingle<User>(query, param: parameters, commandType: CommandType.StoredProcedure);
                return user;
            }
        }
    }
}
