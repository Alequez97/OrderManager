using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Edit
{
    /// <summary>
    /// Interaction logic for EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {

        private readonly ApplicationManager _applicationManager;

        private List<Employee> _employees;
        private List<Order> _orders;
        private List<Product> _products;

        private Dictionary<Product, int> _orderBasket;

        public EditOrder()
        {
            InitializeComponent();
            _applicationManager = ApplicationManager.Instance();

            _employees = _applicationManager.getEmployees();
            _products = _applicationManager.getProducts();
            _orders = _applicationManager.getOrders();
            _orderBasket = new Dictionary<Product, int>();

            FillComboBoxes();
        }



        private void EditOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            var orderSelectedIndex = orderNumberComboBox.SelectedIndex;

            if (orderSelectedIndex == -1)
            {
                orderNumberComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                orderNumberErrorTextBlock.Text = "Please choose order you want to edit";
                return;
            }

            var employeeSelectedIndex = employeeComboBox.SelectedIndex;
            var stateSelectedIndex = stateComboBox.SelectedIndex;

            var response = _applicationManager.EditOrder(_employees[employeeSelectedIndex], (StateEnum)stateSelectedIndex, _orderBasket, _orders[orderSelectedIndex]);
            var responseCode = response.Keys.ElementAt(0);

            if (responseCode == 1)
            {
                responseLabel.Content = response[responseCode];
                responseLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
            }
        }


        private void FillOrderInformation(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = orderNumberComboBox.SelectedIndex;
            var employee = _orders[selectedIndex].ResponsibleEmployee;
            var responsibleEmployeeIndex = GetEmployeeIndexInCollection(employee);

            employeeComboBox.SelectedIndex = responsibleEmployeeIndex;
            stateComboBox.SelectedItem = _orders[selectedIndex].State;

            FillOrderBasketDictionary(_orders[selectedIndex]);
            FillOrderBasket();
        }

        private int GetEmployeeIndexInCollection(Employee employee)
        {
            int index = 0;
            foreach (var e in _employees)
            {
                if (e.PersonalCode.Equals(employee.PersonalCode)) return index;
                index++;
            }

            return -1;
        }

        private void FillOrderBasketDictionary(Order order)
        {
            _orderBasket.Clear();
            foreach (var detail in order.OrderDetails)
            {
                _orderBasket.Add(detail.Product, detail.Amount);
            }
        }

        private void ClearBasket_OnClick(object sender, RoutedEventArgs e)
        {
            _orderBasket.Clear();
            productsListBox.Items.Clear();
        }

        private void FillOrderBasket()
        {
            productsListBox.Items.Clear();
            foreach (var orderBasketProduct in _orderBasket)
            {
                productsListBox.Items.Add(orderBasketProduct.Key + " x " + orderBasketProduct.Value);
            }
        }

        private void AddProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            productsListBox.Items.Clear();
            productErrorTextBlock.Text = "";
            amountErrorTextBlock.Text = "";
            amountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            productComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);

            List<Product> productsInBasket = _orderBasket.Keys.ToList();

            var productIndex = productComboBox.Items.IndexOf(productComboBox.SelectedItem);

            bool isFailed = false;

            if (productIndex == -1)
            {
                productErrorTextBlock.Text = "Error. Choose product";
                productComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                isFailed = true;
            }

            var amountText = amountTextBox.Text.Trim();

            if (amountText.Equals(""))
            {
                amountErrorTextBlock.Text = "Must be int";
                amountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                isFailed = true;
            }
            else
            {
                try
                {
                    Convert.ToInt32(amountText);
                    if (Convert.ToInt32(amountText) <= 0)
                    {
                        amountErrorTextBlock.Text = "Must be > 0";
                        amountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        isFailed = true;
                    }
                }
                catch (Exception)
                {
                    amountErrorTextBlock.Text = "Must be int";
                    amountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    isFailed = true;
                }
            }

            foreach (var orderBasketProduct in _orderBasket)
            {
                productsListBox.Items.Add(orderBasketProduct.Key + " x " + orderBasketProduct.Value);
            }

            if (isFailed) return;

            var productToAdd = _products[productIndex];

            bool contains = false;
            foreach (var productInBasket in productsInBasket)
            {
                if (productInBasket.Name.Equals(productToAdd.Name))
                {
                    _orderBasket[productInBasket] += Convert.ToInt32(amountText);
                    contains = true;
                }
            }

            if (!contains)
            {
                _orderBasket.Add(productToAdd, Convert.ToInt32(amountText));
            }

            productsListBox.Items.Clear();
            foreach (var orderBasketProduct in _orderBasket)
            {
                productsListBox.Items.Add(orderBasketProduct.Key + " x " + orderBasketProduct.Value);
            }
        }

        private void FillComboBoxes()
        {
            _employees.Clear();
            _orders.Clear();
            orderNumberComboBox.Items.Clear();
            employeeComboBox.Items.Clear();
            productComboBox.Items.Clear();
            stateComboBox.Items.Clear();

            _employees = _applicationManager.getEmployees();
            _orders = _applicationManager.getOrders();

            foreach (var order in _orders)
            {
                orderNumberComboBox.Items.Add(order.Number);
            }

            foreach (var employee in _employees)
            {
                employeeComboBox.Items.Add(employee.FullName + " (p.c. - " + employee.PersonalCode + ")");
            }

            foreach (var product in _products)
            {
                productComboBox.Items.Add(product.Name);
            }

            foreach (var state in Enum.GetValues(typeof(StateEnum)))
            {
                stateComboBox.Items.Add(state);
            }
        }
    }
}
