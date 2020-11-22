using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Delete
{
    /// <summary>
    /// Interaction logic for DeleteProduct.xaml
    /// </summary>
    public partial class DeleteProduct : Window
    {

        private readonly ApplicationManager _applicationManager;
        private List<Product> _products;

        public DeleteProduct()
        {
            InitializeComponent();

            _applicationManager = ApplicationManager.Instance();
            _products = _applicationManager.getProducts();

            FillComboBox();
        }

        private void ProductComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            productErrorLabel.Content = "";
        }

        private void DeleteProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteIndex = productComboBox.SelectedIndex;
            responseGrid.Children.Clear();

            if (deleteIndex == -1)
            {
                productComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                productErrorLabel.Content = "Please choose product";
                return;
            }

            var response = _applicationManager.DeleteProduct(_products.ElementAt(deleteIndex));

            FillComboBox();

            //1 - success, 0 - error, -1 - warning
            var responseCode = response.Keys.ElementAt(0);
            SolidColorBrush color;
            if (responseCode == 1)
            {
                color = new SolidColorBrush(Color.FromRgb(0, 140, 0));
            }
            else if (responseCode == -1)
            {
                color = new SolidColorBrush(Color.FromRgb(255, 150, 0));
            }
            else
            {
                color = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            var textBlock = new TextBlock
            {
                Text = $"{response[responseCode]}",
                Foreground = color,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            responseGrid.Children.Add(textBlock);
        }

        private void FillComboBox()
        {
            productComboBox.Items.Clear();
            _products.Clear();
            _products = _applicationManager.getProducts();

            foreach (Product product in _products)
            {
                productComboBox.Items.Add(product.Name);
            }
        }
    }
}
