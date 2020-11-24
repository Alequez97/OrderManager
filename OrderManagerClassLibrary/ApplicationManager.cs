using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using OrderManagerClassLibrary.Interfaces;

namespace OrderManagerClassLibrary
{
    public class ApplicationManager : IApplicationManager
    {
        private static ApplicationManager applicationManager;
        private static OrderManagerDbContext context;

        public static ApplicationManager Instance()
        {
            if (applicationManager == null)
            {
                applicationManager = new ApplicationManager();
                context = new OrderManagerDbContext();
            }
            return applicationManager;
        }

        public List<Employee> Employees
        {
            get
            {
                var employees = context.Employees.Select(x => x).ToList();
                return employees;
            }
        }

        public List<Customer> Customers
        {
            get
            {
                var customers = context.Customers.Select(x => x).ToList();
                return customers;
            }
        }

        public List<Product> Products
        {
            get
            {

                var products = context.Products.Select(x => x).ToList();
                return products;
            }
        }

        public List<Order> Orders
        {
            get
            {
                var orders = context.Orders.Select(x => x).ToList();
                return orders;
            }
        }


        private ApplicationManager()
        {

        }

        public Dictionary<int, string> AddCustomer(string name, string surname, string personalCode, string email)
        {
            Dictionary<int, string> response = new Dictionary<int, string>();

            List<Customer> customers;
            using (var context = new OrderManagerDbContext())
            {
                customers = context.Customers.Select(x => x).ToList();
            }

            bool contains = false;
            foreach (var customer in customers)                      //check if collection of customers contains object 
            {                                                            //with user typed personal code
                if (customer.PersonalCode.Equals(personalCode)) contains = true;
            }

            if (contains)
            {
                Console.WriteLine("Error. Customer with this personal code already exists");
                response.Add(0, "Customer with this personal code already exists");
                return response;
            }
            else                                                        //if not, create new customer and add to collection
            {
                try
                {
                    var address = new MailAddress(email);

                    var customer = new Customer(name, surname, personalCode, email);
                    context.Customers.Add(customer);
                    context.SaveChanges();

                    response.Add(1, "Customer successfully added");
                    return response;

                }
                catch (FormatException)
                {
                    Console.WriteLine("Email address is invalid. Try again");
                    response.Add(0, "Email address is invalid");
                    return response;
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine("Email address is invalid. Try again");
                    response.Add(0, "Error occurred when saving data in database");
                    return response;
                }
                catch (Exception)
                {
                    response.Add(0, "Error occurred. Try again later");
                    return response;
                }

            }

        }

        public Dictionary<int, string> AddEmployee(string name, string surname, string personalCode, string email)
        {

            Dictionary<int, string> response = new Dictionary<int, string>();

            bool contains = false;
            foreach (Employee employee in getEmployees())        //check if collection of employees contains object 
            {                                               //with user typed personal code
                if (employee.PersonalCode.Equals(personalCode)) contains = true;
            }

            if (contains)
            {
                Console.WriteLine("Error. Employee with this personal code already exists");
                response.Add(0, "Employee with that personal code already exists");
                return response;
            }
            else                                                //if not, create new employee and add to collection
            {
                try
                {
                    var address = new MailAddress(email);

                    var employee = new Employee(name, surname, personalCode, email);
                    context.Employees.Add(employee);
                    context.SaveChanges();

                    response.Add(1, "Employee successfully added");
                    return response;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Email address is invalid. Try again");
                    response.Add(0, "Email address is invalid");
                    return response;
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine("Email address is invalid. Try again");
                    response.Add(0, "Error occurred when saving data in database");
                    return response;
                }
                catch (Exception)
                {
                    response.Add(0, "Error occurred. Try again later");
                    return response;
                }

            }

        }

        public Dictionary<int, string> AddOrder(Customer customer, Employee employee, Dictionary<Product, int> orderBasket)
        {

            var order = new Order(customer, employee, DateTime.Now);

            foreach (var orderBasketElement in orderBasket)
            {
                order.AddProduct(orderBasketElement.Key.Name, orderBasketElement.Value);
            }

            try
            {
                context.Orders.Add(order);
                foreach (var orderDetail in order.OrderDetails)
                {
                    context.OrderDetails.Add(orderDetail);
                }
                context.SaveChanges();

                var response = new Dictionary<int, string> { { 1, "Order successfully created" } };
                return response;
            }
            catch
            {
                var response = new Dictionary<int, string> { { 0, "Order can't be added. Try again later" } };
                return response;
            }


        }

        public Dictionary<int, string> AddProduct(string productName, decimal price)
        {

            Dictionary<int, string> response = new Dictionary<int, string>();

            foreach (Product product in getProducts())     //check if collection contains object with user typed product name
            {
                if (product.Name.ToLower().Equals(productName.ToLower()))
                {
                    product.Price = price;
                    response.Add(1, "Product " + productName + " already was in stock. Price was changed");
                    return response;
                }
            }

            //if collection not contains, create new product and save collection

            productName = productName.ToLower();
            char firstLetterUpper = char.ToUpper(productName[0]);
            productName = firstLetterUpper + productName.Substring(1);

            try
            {
                var newProduct = new Product(productName, price);
                context.Products.Add(newProduct);
                context.SaveChanges();
                response.Add(1, "Product successfully added");
                return response;
            }
            catch
            {
                response.Add(0, "Can't save product");
                return response;
            }
        }

        public List<Employee> getEmployees()
        {

            var employees = context.Employees.Select(x => x).ToList();

            return employees;
        }

        public List<Product> getProducts()
        {
            var products = context.Products.Select(x => x).ToList();
            return products;
        }

        public List<Customer> getCustomers()
        {
            var customers = context.Customers.Select(x => x).ToList();
            return customers;
        }

        public List<Order> getOrders()
        {
            var orders = context.Orders.Select(x => x).ToList();
            return orders;
        }

        public List<OrderDetail> GetOrderDetails()
        {
            var orderDetails = context.OrderDetails.Select(x => x).ToList();
            return orderDetails;
        }

        public Dictionary<int, string> EditCustomer(string newName, string newSurname, string newEmail, string oldPersonalCode, int indexInCollection)
        {

            if (!IsValidEmail(newEmail))
            {
                return new Dictionary<int, string> { { 0, "Error. Wrong e-mail format" } };
            }

            var customers = getCustomers();
            var customer = customers.ElementAt(indexInCollection);

            if (customer.Name.Equals(newName) && customer.Surname.Equals(newSurname) &&
                customer.Email.Equals(newEmail))
            {
                return new Dictionary<int, string> { { -1, "No found changes to make" } };
            }

            customers[indexInCollection].Name = newName;
            customers[indexInCollection].Surname = newSurname;
            customers[indexInCollection].Email = newEmail;

            int orderIndex = 0;
            var orders = getOrders();
            foreach (var order in orders)
            {
                if (order.Customer.PersonalCode.Equals(oldPersonalCode))
                {
                    orders[orderIndex].Customer.Name = newName;
                    orders[orderIndex].Customer.Surname = newSurname;
                    orders[orderIndex].Customer.Email = newEmail;
                }

                orderIndex++;
            }

            try
            {
                context.SaveChanges();
                return new Dictionary<int, string>() { { 1, "Customer was successfully modified" } };
            }
            catch (Exception)
            {
                return new Dictionary<int, string> { { 0, "Unable to edit customer. Try again later" } };
            }

        }
        public Dictionary<int, string> EditEmployee(string newName, string newSurname, string newEmail, string oldPersonalCode, int indexInCollection)
        {
            if (!IsValidEmail(newEmail))
            {
                return new Dictionary<int, string> { { 0, "Error. Wrong e-mail format" } };
            }

            var employees = getEmployees();
            var employee = employees.ElementAt(indexInCollection);

            if (employee.Name.Equals(newName) && employee.Surname.Equals(newSurname) &&
                employee.Email.Equals(newEmail))
            {
                return new Dictionary<int, string> { { -1, "No found changes to make" } };
            }

            employees[indexInCollection].Name = newName;
            employees[indexInCollection].Surname = newSurname;
            employees[indexInCollection].Email = newEmail;

            int orderIndex = 0;
            var orders = getOrders();
            foreach (var order in orders)
            {
                if (order.ResponsibleEmployee.PersonalCode.Equals(oldPersonalCode))
                {
                    orders[orderIndex].ResponsibleEmployee.Name = newName;
                    orders[orderIndex].ResponsibleEmployee.Surname = newSurname;
                    orders[orderIndex].ResponsibleEmployee.Email = newEmail;
                }

                orderIndex++;
            }
            try
            {
                context.SaveChanges();
                return new Dictionary<int, string>() { { 1, "Employee was successfully modified" } };
            }
            catch (Exception)
            {
                return new Dictionary<int, string> { { 0, "Unable to edit employee. Try again later" } };
            }

        }

        public Dictionary<int, string> EditProduct(string newName, decimal newPrice, int index)
        {
            int controlIndex = 0;
            var products = getProducts();

            foreach (var product in products)
            {
                if (controlIndex != index)
                {
                    if (product.Name.ToLower().Equals(newName.ToLower())) return new Dictionary<int, string> { { 0, $"Product {newName} already exists" } };
                }
                controlIndex++;
            }

            if (products[index].Name.Equals(newName) && products[index].Price == newPrice) return new Dictionary<int, string> { { -1, "No changes made" } };

            products[index].Name = newName;
            products[index].Price = newPrice;

            try
            {
                context.SaveChanges();
                return new Dictionary<int, string> { { 1, "Product successfully edited" } };
            }
            catch (Exception)
            {
                return new Dictionary<int, string> { { 0, "Unable to edit product. Try again later" } };
            }
        }

        public Dictionary<int, string> EditOrder(Employee newEmployee, StateEnum newState, Dictionary<Product, int> orderBasket, Order order)
        {
            order.ResponsibleEmployee = newEmployee;
            order.State = newState;
            order.OrderDetails.Clear();
            foreach (var item in orderBasket)
            {
                order.OrderDetails.Add(new OrderDetail() { Product = item.Key, Amount = item.Value });
            }

            try
            {
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("delete from OrderDetails where Order_Number is NULL");
                return new Dictionary<int, string> { { 1, "Order successfully edited" } };
            }
            catch
            {
                return new Dictionary<int, string> { { 1, "Wasn't able to edit order. Try again later" } };
            }
        }

        public Dictionary<int, string> DeleteEmployee(Employee employee)
        {
            try
            {
                var ordersList = context.Orders
                .SqlQuery("Select * from Orders where ResponsibleEmployee_PersonalCode = \'" + employee.PersonalCode + "\'").ToList<Order>();
                if (ordersList.Count > 0)
                {
                    return new Dictionary<int, string>
                    {
                        { 0, $"Unable to delete\n" +
                             $"This employee appends in {ordersList.Count} orders\n" +
                             $"Firstly delete this orders, than you'll can delete\n" +
                             $"this employee" }
                    };
                }
                context.Employees.Remove(employee);
                context.SaveChanges();
                return new Dictionary<int, string> { { 1, "Employee deleted" } };
            }
            catch (Exception e)
            {
                return new Dictionary<int, string> { { 0, $"Unable to delete employee\n" +
                                                          $"Try again later" }};
            }
        }

        public Dictionary<int, string> DeleteCustomer(Customer customer)
        {
            try
            {
                var ordersList = context.Orders
               .SqlQuery("Select * from Orders where Customer_PersonalCode = \'" + customer.PersonalCode + "\'").ToList<Order>();
                if (ordersList.Count > 0)
                {
                    return new Dictionary<int, string>
                    {
                        { 0, $"Unable to delete\n" +
                             $"This customer appends in {ordersList.Count} orders\n " +
                             $"Firstly delete this orders, than you'll can delete\n" +
                             $"this customer" }
                    };
                }
                context.Customers.Remove(customer);
                context.SaveChanges();
                return new Dictionary<int, string> { { 1, "Customer deleted" } };
            }
            catch (Exception e)
            {
                return new Dictionary<int, string> { { 0, $"Unable to delete customer\n" +
                                                          $"Try again later" } };
            }
        }

        public Dictionary<int, string> DeleteProduct(Product product)
        {
            try
            {
                var detailsList = context.OrderDetails
                .SqlQuery("Select * from OrderDetails where Product_Id = " + product.Id).ToList<OrderDetail>();
                if (detailsList.Count > 0)
                {
                    return new Dictionary<int, string>
                    {
                        { 0, $"Unable to delete\n" +
                             $"This product appends in {detailsList.Count} orders\n " +
                             $"Firstly delete this orders, than you'll can remove\n" +
                             $"this product" }
                    };
                }
                context.Products.Remove(product);
                context.SaveChanges();
                return new Dictionary<int, string> { { 1, "Product deleted" } };
            }
            catch (Exception e)
            {
                return new Dictionary<int, string> { { 0, $"Unable to delete product\n" +
                                                          $"Try again later" } };
            }
        }

        public Dictionary<int, string> DeleteOrder(Order order)
        {
            try
            {
                context.Orders.Remove(order);
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("delete from OrderDetails where Order_Number is NULL");
                return new Dictionary<int, string> { { 1, "Order deleted" } };
            }
            catch (Exception e)
            {
                return new Dictionary<int, string> { { 0, $"Unable to delete order\n" +
                                                          $"Try again later" } };
            }
        }

        bool IsValidEmail(string email)             //https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
