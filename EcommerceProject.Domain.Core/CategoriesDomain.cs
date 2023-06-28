using EcommerceProject.Domain.Entity;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Infrastructure.Interface;

namespace EcommerceProject.Domain.Core
{
    public class CategoriesDomain : ICategoriesDomain
    {
        private IUnitOfWork _unitOfWork;
        public CategoriesDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            return _unitOfWork.Categories.GetAll();
        }
    }
}
