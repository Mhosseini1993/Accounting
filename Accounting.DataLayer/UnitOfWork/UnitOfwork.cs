using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Repository;
using Accounting.DataLayer.Services;
using Accounting.DataLayer.EF_Model;
using Accounting.DataLayer.GenericRepository;

namespace Accounting.DataLayer.UnitOfWork
{
    public class UnitOfwork : IDisposable
    {
        Accounting_DBEntities db = new Accounting_DBEntities();

        private ICustomerRepository _customerRepository;
        private GenericRepository<AccountingType> _accountingTypeRepository;
        private GenericRepository<Account> _accountRepository;
        private GenericRepository<User> _UserRepository;

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new CustomerRepository(db);

                return _customerRepository;
            }

        }

        public GenericRepository<AccountingType> AccountingTypeRepository
        {
            get
            {
                if (_accountingTypeRepository == null)
                    _accountingTypeRepository = new GenericRepository<AccountingType>(db);
                return _accountingTypeRepository;
            }
        }

        public GenericRepository<Account> AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                    _accountRepository = new GenericRepository<Account>(db);
                return _accountRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_UserRepository == null)
                    _UserRepository = new GenericRepository<User>(db);
                return _UserRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
