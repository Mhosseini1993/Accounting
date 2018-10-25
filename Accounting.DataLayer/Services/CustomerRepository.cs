using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.EF_Model;
using Accounting.DataLayer.Repository;
using Accounting.ViewModel.CustomerViewModels;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private Accounting_DBEntities db;
        public CustomerRepository(Accounting_DBEntities Context)
        {
            this.db = Context;
        }
        public List<Customer> GetAll()
        {
            return db.Customers.OrderByDescending(c => c.ID).ToList();
        }
        public Customer GetByID(int Id)
        {
            return db.Customers.SingleOrDefault(c => c.ID == Id);
        }
        public bool InsertCustomer(Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                if (db.Entry(customer).State == System.Data.Entity.EntityState.Detached)
                    db.Customers.Attach(customer);
                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteCustomer(Customer customer)
        {
            try
            {
                ///1
                ///
                db.Entry(customer).State = System.Data.Entity.EntityState.Deleted;

                ///2
                ///
                //Customer c = db.Customers.Find(customer.ID);
                //db.Customers.Remove(c);

                Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteCustomer(int id)
        {
            try
            {
                DeleteCustomer(GetByID(id));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public List<Customer> Search(string Param)
        {
            return db.Customers.Where(c => c.FullName.Contains(Param) || c.Address.Contains(Param) || c.Mobile.Contains(Param)).ToList();
        }
        public List<ListCustomerViewModel> Getcustomernames(string filter = "")
        {
            if (filter == "")
            {
                return db.Customers.Select(c => new ListCustomerViewModel
                {
                    ID = c.ID,
                    FullName = c.FullName
                }).ToList();
            }
            else
            {
                return db.Customers.Where(c => c.FullName.Contains(filter)).Select(p => new ListCustomerViewModel
                {
                    ID = p.ID,
                    FullName = p.FullName
                }).ToList();
            }
        }
    }
}
