using Dapper;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Domain.Entities;
using EcommerceProject.Persistence.Contexts;
using System.Data;

namespace EcommerceProject.Persistence.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DapperContext _context;
        public CategoriesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var query = "Select * From Categories";

            var categories = await connection.QueryAsync<Category>(query, commandType: CommandType.Text);
            return categories;
        }
    }
}
