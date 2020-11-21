using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Add
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private ApplicationManager applicationManager;

        private Dictionary<Product, int> orderBasket;

        private List<Customer> customers;
        private List<Employee> employees;
        private List<Product> products;

        public AddOrder()
        {
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();

            customers = applicationManager.getCustomers();
            employees = applicationManager.getEmployees();
            products = applicationManager.getProducts();
            orderBasket = new Dictionary<Product, int>();

            FillComboBoxes();
        }

        private void FillComboBoxes()
        {
            foreach (var customer in customers)
            {
                customerComboBox.Items.Add(customer.FullName);

            }

            foreach (var employee in employees)
            {
                employeeComboBox.Items.Add(employee.FullName);
            }

            foreach (var product in applicationManager.getProducts())
            {
                productComboBox.Items.Add(product.Name);
            }
        }

        private void AddOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            var chosenCustomerIndex = customerComboBox.Items.IndexOf(customerComboBox.SelectedItem);
            var chosenEmployeeIndex = employeeComboBox.Items.IndexOf(employeeComboBox.SelectedItem);

            ResetErrorSections();

            bool isFailed = false;
            if (chosenCustomerIndex == -1)
            {
                customerErrorTextBlock.Text = "Customer choose is required";
                customerComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                isFailed = true;
            }

            if (chosenEmployeeIndex == -1)
            {
                employeeErrorTextBlock.Text = "Responsible employee choose is required";
                employeeComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                isFailed = true;
            }

            if (orderBasket.Count == 0)
            {
                productErrorTextBlock.Text = "Error. Add products to basket";
                productComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                isFailed = true;
            }

            if (isFailed) return;
            var response = applicationManager.AddOrder(customers[chosenCustomerIndex], employees[chosenEmployeeIndex], orderBasket);

            Label label = new Label();
            label.Content = response[1];
            responseGrid.Children.Add(label);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
        }

        private void AddProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            productsListBox.Items.Clear();
            productErrorTextBlock.Text = "";
            amountErrorTextBlock.Text = "";
            amountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            productComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);

            List<Product> productsInBasket = orderBasket.Keys.ToList();

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

            foreach (var orderBasketProduct in orderBasket)
            {
                productsListBox.Items.Add(orderBasketProduct.Key + " x " + orderBasketProduct.Value);
            }

            if (isFailed) return;

            var productToAdd = products[productIndex];

            bool contains = false;
            foreach (var productInBasket in productsInBasket)
            {
                if (productInBasket.Name.Equals(productToAdd.Name))
                {
                    orderBasket[productInBasket] += Convert.ToInt32(amountText);
                    contains = true;
                }
            }

            if (!contains)
            {
                orderBasket.Add(productToAdd, Convert.ToInt32(amountText));
            }

            productsListBox.Items.Clear();
            foreach (var orderBasketProduct in orderBasket)
            {
                productsListBox.Items.Add(orderBasketProduct.Key + " x " + orderBasketProduct.Value);
            }
        }

        private void ClearBasket_OnClick(object sender, RoutedEventArgs e)
        {
            orderBasket.Clear();
            productsListBox.Items.Clear();
        }

        private void ResetErrorSections()
        {
            customerErrorTextBlock.Text = "";
            employeeErrorTextBlock.Text = "";
            productErrorTextBlock.Text = "";
            amountErrorTextBlock.Text = "";
            productComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            customerComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            employeeComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            amountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        }
    }
}
