using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.EF_Model;
using Accounting.DataLayer.Repository;
using Accounting.DataLayer.Services;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ICustomerRepository customer = new CustomerRepository();
            customer.InsertCustomer(new Customer()
            {
                FullName="Mostafa Hossein",
                Address="Malayer",
                Email="HosseiniMostafa71@gmail.com",
                ImageName="2.jpg",
                Mobile="09358758908"
            });
            var lam = customer.GetAll();

            Console.ReadLine();
        }
    }
}
