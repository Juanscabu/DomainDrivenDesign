﻿using EcommerceProject.Application.Interface.Persistence;

namespace EcommerceProject.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICustomersRepository Customers { get; }

        public IUsersRepository Users { get; }

        public ICategoriesRepository Categories { get; }

        public UnitOfWork(ICustomersRepository customers, IUsersRepository users, ICategoriesRepository categories)
        {
            Customers = customers;
            Users = users;
            Categories = categories;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}