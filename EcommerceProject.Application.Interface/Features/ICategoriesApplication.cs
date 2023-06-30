using EcommerceProject.Application.DTO;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Interface.Features
{
    public interface ICategoriesApplication
    {
        Task<Response<IEnumerable<CategoryDto>>> GetAll();
    }
}
