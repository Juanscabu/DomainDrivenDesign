using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Domain.Interface
{
    public interface ICustomersDomain
    {
        #region sync methods
        bool Insert(Customer customer);

        bool Update(Customer customer);

        bool Delete(string customerId);

        Customer Get(string customerId);

        IEnumerable<Customer> GetAll();

        IEnumerable<Customer> GetAllWithPagination(int PageNumber, int PageSize);

        int Count();
        #endregion

        #region async methods
        Task<bool> InsertAsync(Customer customer);

        Task<bool> UpdateAsync(Customer customer);

        Task<bool> DeleteAsync(string customerId);

        Task<Customer> GetAsync(string customerId);

        Task<IEnumerable<Customer>> GetAllAsync();

        Task<IEnumerable<Customer>> GetAllWithPaginationAsync(int PageNumber, int PageSize);

        Task<int> CountAsync();
        #endregion
    }
}
