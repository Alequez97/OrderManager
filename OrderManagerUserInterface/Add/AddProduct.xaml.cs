using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderManagerUserInterface.Add
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {

        private ApplicationManager applicationManager;

        public AddProduct()
        {
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();
        }

        private void AddProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            string priceString = priceTextBox.Text.Trim();

            SetLabelsEmpty();
            SetDefaultBackgroundForTextBoxes();
            responseGrid.Children.Clear();

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

            Dictionary<int, string> response = applicationManager.AddProduct(name, price);

            responseGrid.Children.Clear();
            responseGrid.HorizontalAlignment = HorizontalAlignment.Center;
            Label responseLabel = new Label();
            responseLabel.Content = response[1];
            responseLabel.HorizontalAlignment = HorizontalAlignment.Center;
            responseLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
            responseLabel.HorizontalAlignment = HorizontalAlignment.Center;
            responseGrid.Children.Add(responseLabel);

            SetTextBoxesEmpty();
        }

        private void SetDefaultBackgroundForTextBoxes()
        {
            nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            priceTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        }

        private void SetLabelsEmpty()
        {
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
