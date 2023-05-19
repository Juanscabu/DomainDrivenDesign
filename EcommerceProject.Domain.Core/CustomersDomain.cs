using EcommerceProject.Domain.Entity;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Infrastructure.Interface;

namespace EcommerceProject.Domain.Core
{
    public class CustomerDomain : ICustomersDomain
    {
        private readonly ICustomersRepository _customerRepository;

        public CustomerDomain(ICustomersRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #region sync methods

        public bool Insert(Customer customer)
        {
            return _customerRepository.Insert(customer);
        }

        public bool Update(Customer customer) 
        { 
            return (_customerRepository.Update(customer));
        }

        public bool Delete(string customerId)
        {
            return _customerRepository.Delete(customerId);
        }

        public Customer Get(string customerId)
        { 
            return _customerRepository.Get(customerId);
        }

        public IEnumerable<Customer> GetAll()
        { 
        return _customerRepository.GetAll(); 
        }

        #endregion

        #region async methods
        public async Task<bool> InsertAsync(Customer customer)
        { 
            return await _customerRepository.InsertAsync(customer);
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            return await _customerRepository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(string customerId)
        {
            return await _customerRepository.DeleteAsync(customerId);
        }

        public async Task<Customer> GetAsync(string customerId)
        {
            return await _customerRepository.GetAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        { 
            return await _customerRepository.GetAllAsync(); 
        }

        #endregion
    }

}