using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Infrastructure.Interface
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
