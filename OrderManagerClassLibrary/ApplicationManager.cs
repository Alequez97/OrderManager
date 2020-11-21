using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using OrderManagerClassLibrary.Interfaces;

namespace OrderManagerClassLibrary
{
    public class ApplicationManager : IApplicationManager
    {
        private static ApplicationManager applicationManager;

        private DataManager dataManager;

        private List<Employee> employees;
        private List<Customer> customers;
        private List<Product> products;
        private List<Order> orders;

        public List<Employee> Employees
        {
            get
            {
                dataManager = DataManager.Instance();
                dataManager.Load();
                return employees;
            }
            set
            {
                employees = value;
            }
        }

        public List<Customer> Customers
        {
            get
            {
                dataManager = DataManager.Instance();
                dataManager.Load();
                return customers;
            }
            set
            {
                customers = value;
            }
        }

        public List<Product> Products
        {
            get
            {
                dataManager = DataManager.Instance();
                dataManager.Load();
                return products;
            }
            set
            {
                products = value;
            }
        }

        public List<Order> Orders
        {
            get
            {
                dataManager = DataManager.Instance();
                dataManager.Load();
                return orders;
            }
            set
            {
                orders = value;
            }
        }

        public static ApplicationManager Instance()     
        {
            if (applicationManager == null)
            {
                applicationManager = new ApplicationManager();
            }
            return applicationManager;
        }

        private ApplicationManager()
        {
            employees = new List<Employee>();
            customers = new List<Customer>();
            products = new List<Product>();
            orders = new List<Order>();
        }

        public Dictionary<int, string> AddCustomer(string name, string surname, string personalCode, string email)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            Dictionary<int, string> response = new Dictionary<int, string>();

            bool contains = false;
            foreach (Customer customer in customers)                      //check if collection of customers contains object 
            {                                                            //with user typed personal code
                if (customer.PersonalCode.Equals(personalCode)) contains = true;
            }

            if (contains)
            {
                Console.WriteLine("Error. Customer with this personal code already exists");
                response.Add(0, "Customer with this personal code already exists");
                return response;
            }
            else                                                        //if not, create new customer and add to colection
            {
                try
                {
                    var address = new MailAddress(email);
                    customers.Add(new Customer(name, surname, personalCode, email));
                    dataManager.saveCustomers(ref customers);
                    response.Add(1, "Customer successfully added");
                    return response;

                }
                catch (FormatException)
                {
                    Console.WriteLine("Email address is invalid. Try again");
                    response.Add(0, "Email address is invalid");
                    return response;
                }

            }

        }

        public Dictionary<int, string> AddEmployee(string name, string surname, string personalCode, string email)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            Dictionary<int, string> response = new Dictionary<int, string>();

            bool contains = false;
            foreach (Employee employee in employees)        //check if collection of employees contains object 
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
                    Employees.Add(new Employee(name, surname, personalCode, email));
                    dataManager.saveEmployees(ref employees);

                    response.Add(1, "Employee successfully added");
                    return response;
                }
                catch (Exception)
                {
                    Console.WriteLine("Email address is invalid. Try again");
                    response.Add(0, "Invalid e-mail address");
                    return response;
                }

            }

        }

        public Dictionary<int, string> AddOrder(Customer customer, Employee employee, Dictionary<Product, int> orderBasket)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            var order = new Order(customer, employee, DateTime.Now);

            foreach (var orderBasketElement in orderBasket)
            {
                order.addProduct(orderBasketElement.Key.Name, orderBasketElement.Value);
            }

            orders.Add(order);
            dataManager.saveOrders(ref orders);
            var response = new Dictionary<int, string> { { 1, "Order successfully created" } };
            return response;
        }

        public Dictionary<int, string> AddProduct(string productName, decimal price)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            Dictionary<int, string> response = new Dictionary<int, string>();

            foreach (Product product in products)     //check if collection contains object with user typed product name
            {
                if (product.Name.ToLower().Equals(productName.ToLower()))
                {
                    product.Price = price;
                    dataManager.saveProducts(ref products);
                    response.Add(1, "Product " + productName + " already was in stock. Price was changed");
                    return response;
                }
            }

            //if collection not contains, create new product and save collection

            productName = productName.ToLower();
            char firstLetterUpper = char.ToUpper(productName[0]);
            productName = firstLetterUpper + productName.Substring(1);
            products.Add(new Product(productName, price));
            dataManager.saveProducts(ref products);
            response.Add(1, "Product successfully added");
            return response;
        }

        public List<Employee> getEmployees()
        {
            dataManager = DataManager.Instance();
            dataManager.Load();
            return employees;
        }

        public List<Product> getProducts()
        {
            dataManager = DataManager.Instance();
            dataManager.Load();
            return products;
        }

        public List<Customer> getCustomers()
        {
            dataManager = DataManager.Instance();
            dataManager.Load();
            return customers;
        }

        public List<Order> getOrders()
        {
            dataManager = DataManager.Instance();
            dataManager.Load();
            return orders;
        }

        public Dictionary<int, string> EditCustomer(string newName, string newSurname, string newEmail, string newPersonalCode, string oldPersonalCode, int indexInCollection)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            if (!IsValidEmail(newEmail))
            {
                return new Dictionary<int, string> { { 0, "Error. Wrong e-mail format" } };
            }

            var customer = customers.ElementAt(indexInCollection);

            if (customer.Name.Equals(newName) && customer.Surname.Equals(newSurname) &&
                customer.Email.Equals(newEmail) && customer.PersonalCode.Equals(newPersonalCode))
            {
                return new Dictionary<int, string> { { -1, "No found changes to make" } };
            }

            int controlIndex = 0;
            foreach (Customer c in customers)        //check if collection of employees contains object 
            {                                               //with user typed personal code
                if (controlIndex != indexInCollection)
                {
                    if (c.PersonalCode.Equals(newPersonalCode)) return new Dictionary<int, string> { { 0, "Error, customer with this personal code exists" } };
                }
                controlIndex++;
            }

            customers[indexInCollection].Name = newName;
            customers[indexInCollection].Surname = newSurname;
            customers[indexInCollection].Email = newEmail;
            customers[indexInCollection].PersonalCode = newPersonalCode;

            int orderIndex = 0;
            foreach (var order in orders)
            {
                if (order.Customer.PersonalCode.Equals(oldPersonalCode))
                {
                    orders[orderIndex].Customer.Name = newName;
                    orders[orderIndex].Customer.Surname = newSurname;
                    orders[orderIndex].Customer.Email = newEmail;
                    orders[orderIndex].Customer.PersonalCode = newPersonalCode;
                }

                orderIndex++;
            }
            dataManager.saveCustomers(ref customers);
            dataManager.saveOrders(ref orders);
            return new Dictionary<int, string>() { { 1, "Customer was successfully modified" } };

        }
        public Dictionary<int, string> EditEmployee(string newName, string newSurname, string newEmail, string newPersonalCode, string oldPersonalCode, int indexInCollection)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            if (!IsValidEmail(newEmail))
            {
                return new Dictionary<int, string> { { 0, "Error. Wrong e-mail format" } };
            }

            var employee = employees.ElementAt(indexInCollection);

            if (employee.Name.Equals(newName) && employee.Surname.Equals(newSurname) &&
                employee.Email.Equals(newEmail) && employee.PersonalCode.Equals(newPersonalCode))
            {
                return new Dictionary<int, string> { { -1, "No found changes to make" } };
            }

            int controlIndex = 0;
            foreach (Employee e in employees)        //check if collection of employees contains object 
            {                                               //with user typed personal code
                if (controlIndex != indexInCollection)
                {
                    if (e.PersonalCode.Equals(newPersonalCode)) return new Dictionary<int, string> { { 0, "Error, employee with this personal code exists" } };
                }
                controlIndex++;
            }

            employees[indexInCollection].Name = newName;
            employees[indexInCollection].Surname = newSurname;
            employees[indexInCollection].Email = newEmail;
            employees[indexInCollection].PersonalCode = newPersonalCode;

            int orderIndex = 0;
            foreach (var order in orders)
            {
                if (order.ResponsibleEmployee.PersonalCode.Equals(oldPersonalCode))
                {
                    orders[orderIndex].ResponsibleEmployee.Name = newName;
                    orders[orderIndex].ResponsibleEmployee.Surname = newSurname;
                    orders[orderIndex].ResponsibleEmployee.Email = newEmail;
                    orders[orderIndex].ResponsibleEmployee.PersonalCode = newPersonalCode;
                }

                orderIndex++;
            }
            dataManager.saveEmployees(ref employees);
            dataManager.saveOrders(ref orders);
            return new Dictionary<int, string>() { { 1, "Employee was successfully modified" } };

        }

        public Dictionary<int, string> EditProduct(string newName, decimal newPrice, int index)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            int controlIndex = 0;
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

            dataManager.saveProducts(ref products);
            return new Dictionary<int, string> { { 1, "Product successfully edited" } };
        }

        public Dictionary<int, string> EditOrder(Employee newEmployee, State newState, int editOrderIndex)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            orders[editOrderIndex].ResponsibleEmployee = newEmployee;
            orders[editOrderIndex].State = newState;
            dataManager.saveOrders(ref orders);
            return new Dictionary<int, string> { { 1, "Order successfully edited" } };
        }

        public Dictionary<int, string> DeleteEmployee(int index)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            employees.RemoveAt(index);
            dataManager.saveEmployees(ref employees);

            return new Dictionary<int, string> { { 1, "Employee deleted" } };
        }

        public Dictionary<int, string> DeleteCustomer(int index)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            customers.RemoveAt(index);
            dataManager.saveCustomers(ref customers);

            return new Dictionary<int, string> { { 1, "Customer deleted" } };
        }

        public Dictionary<int, string> DeleteProduct(int index)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();

            products.RemoveAt(index);
            dataManager.saveProducts(ref products);

            return new Dictionary<int, string> { { 1, "Product deleted" } };
        }

        public Dictionary<int, string> DeleteOrder(Order order)
        {
            dataManager = DataManager.Instance();
            dataManager.Load();


            orders.Remove(order);
            dataManager.saveOrders(ref orders);

            return new Dictionary<int, string> { { 1, "Order deleted" } };
        }

        bool IsValidEmail(string email) //https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
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
