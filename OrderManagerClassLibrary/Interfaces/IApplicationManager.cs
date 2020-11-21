﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Interfaces
{
    public interface IApplicationManager
    {
        public Dictionary<int, string> AddEmployee(string name, string surname, string personalCode, string email);

        public Dictionary<int, string> AddCustomer(string name, string surname, string personalCode, string email);

        public Dictionary<int, string> AddOrder(Customer customer, Employee employee, Dictionary<Product, int> orderBasket);

        public Dictionary<int, string> AddProduct(string productName, decimal price);

        public List<Employee> getEmployees();

        public List<Product> getProducts();

        public List<Customer> getCustomers();

        public List<Order> getOrders();

        public Dictionary<int, string> EditEmployee(string newName, string newSurname, string newEmail, string newPersonalCode, string oldPersonalCode, int indexInCollection);

        public Dictionary<int, string> EditCustomer(string newName, string newSurname, string newEmail, string newPersonalCode, string oldPersonalCode, int indexInCollection);

        public Dictionary<int, string> EditProduct(string newName, decimal newPrice, int indexInCollection);

        public Dictionary<int, string> EditOrder(Employee newEmployee, State newState, int editOrderIndex);

        public Dictionary<int, string> DeleteEmployee(int index);
        public Dictionary<int, string> DeleteCustomer(int index);
        public Dictionary<int, string> DeleteProduct(int index);
        public Dictionary<int, string> DeleteOrder(Order order);
    }
}
