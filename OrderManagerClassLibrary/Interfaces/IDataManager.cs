using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Interfaces
{
    public interface IDataManager
    {

        public void saveEmployees(ref List<Employee> productList);

        public void saveCustomers(ref List<Customer> productList);

        public void saveOrders(ref List<Order> productList);

        public void saveProducts(ref List<Product> productList);

        public void Load();

        public void Print();

    }
}
