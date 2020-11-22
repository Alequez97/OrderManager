using System.Windows;
using OrderManagerClassLibrary;
using OrderManagerUserInterface.Add;
using OrderManagerUserInterface.Delete;
using OrderManagerUserInterface.Edit;
using OrderManagerUserInterface.Show;

namespace OrderManagerUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly ApplicationManager _applicationManager;

        public MainWindow()
        {
            InitializeComponent();
            _applicationManager = ApplicationManager.Instance();
        }

        #region buttons_that_show_information
        private void ShowEmployees_OnClick(object sender, RoutedEventArgs e)
        {
            var showEmployees = new ShowEmployees(_applicationManager.getEmployees());
            showEmployees.Show();
        }

        private void ShowCustomers_OnClick(object sender, RoutedEventArgs e)
        {
            var showCustomers = new ShowCustomers(_applicationManager.getCustomers());
            showCustomers.Show();
        }

        private void ShowProducts_OnClick(object sender, RoutedEventArgs e)
        {
            var showProducts = new ShowProducts(_applicationManager.getProducts());
            showProducts.Show();
        }

        private void ShowOrders_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationManager.getCustomers();
            _applicationManager.getEmployees();
            _applicationManager.getProducts();
            _applicationManager.GetOrderDetails();
          
            var showOrders = new ShowOrders(_applicationManager.getOrders());
            showOrders.Show();
        }

        #endregion

        private void AddEmployees_OnClick(object sender, RoutedEventArgs e)
        {
            var addEmployeeFrom = new AddPerson(new Employee());
            addEmployeeFrom.Show();
        }


        private void AddCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var addCustomerForm = new AddPerson(new Customer());
            addCustomerForm.Show();
        }

        private void AddProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var addProductForm = new AddProduct();
            addProductForm.Show();
        }

        private void AddOrders_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationManager.getCustomers();
            _applicationManager.getEmployees();
            _applicationManager.getProducts();
            _applicationManager.GetOrderDetails();

            var addOrderForm = new AddOrder();
            addOrderForm.Show();
        }

        private void EditEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            var editEmployeeFrom = new EditPerson(new Employee());
            editEmployeeFrom.Show();
        }

        private void EditCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var editCustomerFrom = new EditPerson(new Customer());
            editCustomerFrom.Show();
        }

        private void EditProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var editProductForm = new EditProduct();
            editProductForm.Show();
        }

        private void EditOrders_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationManager.getCustomers();
            _applicationManager.getEmployees();
            _applicationManager.getProducts();
            _applicationManager.GetOrderDetails();

            var editOrdersForm = new EditOrder();
            editOrdersForm.Show();
        }

        private void DeleteEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteEmployeeForm = new DeletePerson(new Employee());
            deleteEmployeeForm.Show();
        }
        private void DeleteCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteCustomerForm = new DeletePerson(new Customer());
            deleteCustomerForm.Show();
        }
        private void DeleteProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteProductForm = new DeleteProduct();
            deleteProductForm.Show();
        }

        private void DeleteOrders_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationManager.getCustomers();
            _applicationManager.getEmployees();
            _applicationManager.getProducts();
            _applicationManager.GetOrderDetails();

            var deleteOrderForm = new DeleteOrder();
            deleteOrderForm.Show();
        }
    }
}
