using EcommerceProject.Application.DTO;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Interface
{
    public interface ICategoriesApplication
    {
        Task<Response<IEnumerable<CategoryDto>>> GetAll();
    }
}
