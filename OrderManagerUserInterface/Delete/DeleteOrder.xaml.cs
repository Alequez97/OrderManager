using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Delete
{
    /// <summary>
    /// Interaction logic for DeleteOrder.xaml
    /// </summary>
    public partial class DeleteOrder : Window
    {

        private ApplicationManager applicationManager;
        private List<Order> orders;

        public DeleteOrder()
        {
            InitializeComponent();

            applicationManager = ApplicationManager.Instance();
            orders = applicationManager.getOrders();

            FillComboBox();
        }

        private void OrderNumberComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeleteOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteIndex = orderNumberComboBox.SelectedIndex;
            responseGrid.Children.Clear();

            if (deleteIndex == -1)
            {
                orderNumberComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                orderNumberErrorTextBlock.Text = "Please choose order number";
                return;
            }

            var response = applicationManager.DeleteOrder(orders[deleteIndex]);

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
            orderNumberComboBox.Items.Clear();
            orders.Clear();
            orders = applicationManager.getOrders();

            foreach (var order in orders)
            {
                orderNumberComboBox.Items.Add(order.Number);
            }
        }
    }
}
