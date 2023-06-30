using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Application.Interface.Persistence
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
