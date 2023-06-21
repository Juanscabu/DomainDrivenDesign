using EcommerceProject.Domain.Entity;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Infrastructure.Interface;

namespace EcommerceProject.Domain.Core
{
    public class CustomersDomain : ICustomersDomain
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region sync methods

        public bool Insert(Customer customer)
        {
            return _unitOfWork.Customers.Insert(customer);
        }

        public bool Update(Customer customer) 
        { 
            return (_unitOfWork.Customers.Update(customer));
        }

        public bool Delete(string customerId)
        {
            return _unitOfWork.Customers.Delete(customerId);
        }

        public Customer Get(string customerId)
        { 
            return _unitOfWork.Customers.Get(customerId);
        }

        public IEnumerable<Customer> GetAll()
        { 
            return _unitOfWork.Customers.GetAll(); 
        }

        public IEnumerable<Customer> GetAllWithPagination(int PageNumber, int PageSize)
        {
            return _unitOfWork.Customers.GetAllWithPagination(PageNumber, PageSize);
        }

        public int Count()
        {
            return _unitOfWork.Customers.Count();
        }

        #endregion

        #region async methods
        public async Task<bool> InsertAsync(Customer customer)
        { 
            return await _unitOfWork.Customers.InsertAsync(customer);
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            return await _unitOfWork.Customers.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(string customerId)
        {
            return await _unitOfWork.Customers.DeleteAsync(customerId);
        }

        public async Task<Customer> GetAsync(string customerId)
        {
            return await _unitOfWork.Customers.GetAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        { 
            return await _unitOfWork.Customers.GetAllAsync(); 
        }

        public async Task<IEnumerable<Customer>> GetAllWithPaginationAsync(int PageNumber, int PageSize)
        {
            return await _unitOfWork.Customers.GetAllWithPaginationAsync(PageNumber, PageSize);
        }

        public async Task<int> CountAsync()
        {
            return await _unitOfWork.Customers.CountAsync();
        }

        #endregion
    }

}