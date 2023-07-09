using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Persistence.Contexts;

namespace EcommerceProject.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ICustomersRepository Customers { get; }

        public IUsersRepository Users { get; }

        public ICategoriesRepository Categories { get; }

        public IDiscountRepository Discounts { get; }

        public UnitOfWork(ICustomersRepository customers, IUsersRepository users, ICategoriesRepository categories
            , IDiscountRepository discounts, ApplicationDbContext applicationDbContext)
        {
            Customers = customers;
            Users = users;
            Categories = categories;
            Discounts = discounts;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
