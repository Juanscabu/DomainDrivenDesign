using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProject.Infrastructure.Interface
{
    public interface IUnitOfWork: IDisposable
    {
        ICustomersRepository Customers { get; }
        IUsersRepository Users { get; }
        ICategoriesRepository Categories { get; }
    }
}
