using EcommerceProject.Application.DTO;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Interface.Features
{
    public interface IDiscountsApplication
    {
        Task<Response<bool>> Create(DiscountDto discountDto, CancellationToken cancellationToken = default);

        Task<Response<bool>> Update(DiscountDto discountDto, CancellationToken cancellationToken = default);

        Task<Response<bool>> Delete(int id, CancellationToken cancellationToken = default);

        Task<Response<DiscountDto>> Get(int id, CancellationToken cancellationToken = default);

        Task<Response<IEnumerable<DiscountDto>>> GetAll(CancellationToken cancellationToken = default);
    }
}
