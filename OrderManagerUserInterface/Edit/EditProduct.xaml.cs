using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Edit
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {

        private ApplicationManager applicationManager;
        private List<Product> products;

        public EditProduct()
        {
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();
            products = applicationManager.getProducts();

            FillComboBox();
        }

        private void FillTextBoxesWithProductNames(object sender, SelectionChangedEventArgs e)
        {
            if (productComboBox.SelectedIndex != -1)
            {
                nameTextBox.Text = products[productComboBox.SelectedIndex].Name;
                priceTextBox.Text = products[productComboBox.SelectedIndex].Price.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                nameTextBox.Text = "";
                priceTextBox.Text = "";
            }
        }

        private void EditProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            var name = nameTextBox.Text.Trim();
            var priceString = priceTextBox.Text.Trim();
            var selectedIndex = productComboBox.SelectedIndex;

            SetLabelsEmpty();
            SetDefaultBackgroundForTextBoxes();
            responseGrid.Children.Clear();

            if (selectedIndex == -1)
            {
                productComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                productErrorLabel.Content = "Please choose product";
                return;
            }

            bool inputFail = false;
            if (name.Equals(""))
            {
                nameLabel.Content = "Name field is required";
                nameLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputFail = true;
            }

            decimal price = 0;
            try
            {
                price = Convert.ToDecimal(priceString);
            }
            catch (Exception)
            {
                priceLabel.Content = "Price must be numeric";
                priceLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                priceTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputFail = true;
            }

            if (inputFail) return;

            Dictionary<int, string> response = applicationManager.EditProduct(name, price, selectedIndex);
            var responseCode = response.Keys.ElementAt(0);

            var color = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            switch (responseCode)
            {
                case -1:
                    color = new SolidColorBrush(Color.FromRgb(255, 150, 0));
                    break;
                case 0:
                    color = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    break;
                case 1:
                    color = new SolidColorBrush(Color.FromRgb(0, 140, 0));
                    break;
            }

            responseGrid.Children.Clear();
            responseGrid.HorizontalAlignment = HorizontalAlignment.Center;
            Label responseLabel = new Label();
            responseLabel.Content = response[responseCode];
            responseLabel.HorizontalAlignment = HorizontalAlignment.Center;
            responseLabel.Foreground = color;
            responseLabel.HorizontalAlignment = HorizontalAlignment.Center;
            responseGrid.Children.Add(responseLabel);

            if (responseCode == 1)
            {
                SetTextBoxesEmpty();
                FillComboBox();
                //MessageBox.Show("Product was successfully edited!");
            }

        }

        private void FillComboBox()
        {
            productComboBox.Items.Clear();
            products.Clear();
            products = applicationManager.getProducts();

            foreach (Product product in products)
            {
                productComboBox.Items.Add(product.Name);
            }

            productComboBox.SelectedIndex = -1;
        }

        private void SetDefaultBackgroundForTextBoxes()
        {
            nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            priceTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            productComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void SetLabelsEmpty()
        {
            productErrorLabel.Content = "";
            nameLabel.Content = "";
            priceLabel.Content = "";
        }
        private void SetTextBoxesEmpty()
        {
            nameTextBox.Text = "";
            priceTextBox.Text = "";
        }
    }
}
