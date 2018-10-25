using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.EF_Model;
using Accounting.ViewModel.CustomerViewModels;

namespace Accounting.DataLayer.Repository
{
    public interface ICustomerRepository
    {
        List<ListCustomerViewModel> Getcustomernames(string filter="");
        List<Customer> GetAll();
        List<Customer> Search(string Param);
        Customer GetByID(int id);
        bool InsertCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(Customer customer);
        bool DeleteCustomer(int id);
        void Save();
       
    }
}
