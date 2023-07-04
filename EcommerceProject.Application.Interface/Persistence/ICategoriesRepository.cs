using EcommerceProject.Domain.Entities;

namespace EcommerceProject.Application.Interface.Persistence
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
