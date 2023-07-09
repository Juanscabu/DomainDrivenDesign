using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProject.Application.Interface.Persistence
{
    public interface IUnitOfWork: IDisposable
    {
        ICustomersRepository Customers { get; }
        IUsersRepository Users { get; }
        ICategoriesRepository Categories { get; }
        IDiscountRepository Discounts { get; }

        Task<int> Save(CancellationToken cancellationToken);
    }
}
