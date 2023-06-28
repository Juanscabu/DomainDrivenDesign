using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Domain.Interface
{
    public interface ICategoriesDomain
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
