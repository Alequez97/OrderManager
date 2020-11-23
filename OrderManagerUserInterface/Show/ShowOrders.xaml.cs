using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Show
{
    /// <summary>
    /// Interaction logic for ShowOrders.xaml
    /// </summary>
    public partial class ShowOrders : Window
    {
        private int controlIndex;
        private List<Order> orders;

        public ShowOrders(List<Order> orders)
        {
            InitializeComponent();
            controlIndex = 0;
            this.orders = orders;
            PrintOrderInformation(controlIndex);
        }

        public void PrintOrderInformation(int index)
        {
            ordersStackPanel.Children.Clear();

            if (orders.Count == 0 || orders == null)
            {
                Label label = new Label();
                label.Content = "No orders added yet!";
                label.FontSize = 30;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                ordersStackPanel.Children.Add(label);
            }
            else
            {
                var order = orders[index];

                if (orders.Count > 1)
                {
                    var orderCountGrid = new Grid();
                    var orderCountTextBlock = new TextBlock { Text = $"Total orders: {orders.Count}, (current: {index + 1})" };
                    orderCountTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    orderCountTextBlock.FontSize = 15;

                    orderCountGrid.Children.Add(orderCountTextBlock);
                    ordersStackPanel.Children.Add(orderCountGrid);

                    var buttonsGrid = new Grid();
                    buttonsGrid.Margin = new Thickness(0, 0, 0, 10);
                    var cd1 = new ColumnDefinition();
                    var cd2 = new ColumnDefinition();
                    cd1.Width = new GridLength(1, GridUnitType.Star);
                    cd2.Width = new GridLength(1, GridUnitType.Star);
                    buttonsGrid.ColumnDefinitions.Add(cd1);
                    buttonsGrid.ColumnDefinitions.Add(cd2);

                    var previousButton = new Button();
                    previousButton.Content = "Previous order";
                    previousButton.FontSize = 17;
                    previousButton.Margin = new Thickness(90, 0, 90, 0);
                    previousButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(PreviousButton_Click));
                    Grid.SetColumn(previousButton, 0);
                    buttonsGrid.Children.Add(previousButton);

                    var nextButton = new Button();
                    nextButton.Content = "Next order";
                    nextButton.FontSize = 17;
                    nextButton.Margin = new Thickness(90, 0, 90, 0);
                    nextButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(NextButton_Click));
                    Grid.SetColumn(nextButton, 1);
                    buttonsGrid.Children.Add(nextButton);

                    ordersStackPanel.Children.Add(buttonsGrid);

                }

                //creating order header with order number
                var orderNumberGrid = new Grid();
                var colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                orderNumberGrid.ColumnDefinitions.Add(colDef);

                var orderNumberTextBlock = new TextBlock();
                orderNumberTextBlock.Text = "Order number " + order.Number;
                orderNumberTextBlock.FontSize = 25;
                orderNumberTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumn(orderNumberTextBlock, 0);
                orderNumberGrid.Children.Add(orderNumberTextBlock);
                ordersStackPanel.Children.Add(orderNumberGrid);
                //end of creating order header with order number

                //draw horizontal line
                var separator = new Separator();
                ordersStackPanel.Children.Add(separator);
                //end drawing horizontal line

                //draw order date and state
                var orderDateAndStateGrid = new Grid();
                var colDef1 = new ColumnDefinition();
                var colDef2 = new ColumnDefinition();
                colDef1.Width = new GridLength(1, GridUnitType.Star);
                colDef2.Width = new GridLength(1, GridUnitType.Star);
                orderDateAndStateGrid.ColumnDefinitions.Add(colDef1);
                orderDateAndStateGrid.ColumnDefinitions.Add(colDef2);

                var orderDateTextBlock = new TextBlock();
                orderDateTextBlock.Text = "Date: " + order.OrderDate.ToString(CultureInfo.CurrentCulture);
                orderDateTextBlock.FontSize = 15;
                orderDateTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumn(orderDateTextBlock, 0);
                var orderStateTextBlock = new TextBlock();
                orderStateTextBlock.Text = "State: " + order.State.ToString();
                orderStateTextBlock.FontSize = 15;
                orderStateTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumn(orderStateTextBlock, 1);

                orderDateAndStateGrid.Children.Add(orderDateTextBlock);
                orderDateAndStateGrid.Children.Add(orderStateTextBlock);

                ordersStackPanel.Children.Add(orderDateAndStateGrid);
                //end drawing order date and state

                //drawing "Details" line
                var detailsTextBlock = new TextBlock();
                detailsTextBlock.Text = "Details:";
                detailsTextBlock.FontSize = 20;
                detailsTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                detailsTextBlock.Margin = new Thickness(0, 0, 0, 5);
                ordersStackPanel.Children.Add(detailsTextBlock);
                //end drawing "Details line"

                //drawing products grid
                decimal totalSum = 0;
                foreach (var detail in order.OrderDetails)
                {
                    var productsGrid = new Grid();
                    colDef1 = new ColumnDefinition();
                    colDef2 = new ColumnDefinition();
                    var colDef3 = new ColumnDefinition();
                    colDef1.Width = new GridLength(1, GridUnitType.Star);
                    colDef2.Width = new GridLength(1, GridUnitType.Star);
                    colDef3.Width = new GridLength(1, GridUnitType.Star);
                    productsGrid.ColumnDefinitions.Add(colDef1);
                    productsGrid.ColumnDefinitions.Add(colDef2);
                    productsGrid.ColumnDefinitions.Add(colDef3);

                    var productNameTextBlock = new TextBlock { Text = "Product: " + detail.Product.Name, FontSize = 15 };
                    var priceTextBlock = new TextBlock { Text = "Price: " + detail.Product.Price + " $", FontSize = 15 };
                    var amountTextBlock = new TextBlock { Text = "Amount: " + detail.Amount, FontSize = 15 };

                    totalSum += detail.Product.Price * detail.Amount;

                    Grid.SetColumn(productNameTextBlock, 0);
                    Grid.SetColumn(amountTextBlock, 1);
                    Grid.SetColumn(priceTextBlock, 2);

                    productsGrid.Children.Add(productNameTextBlock);
                    productsGrid.Children.Add(priceTextBlock);
                    productsGrid.Children.Add(amountTextBlock);

                    ordersStackPanel.Children.Add(productsGrid);

                }
                //end drawing products grid

                //drawing total sum grid
                var totalSumGrid = new Grid();
                colDef1 = new ColumnDefinition();
                colDef2 = new ColumnDefinition();
                var colDefin3 = new ColumnDefinition();
                colDef1.Width = new GridLength(1, GridUnitType.Star);
                colDef2.Width = new GridLength(1, GridUnitType.Star);
                colDefin3.Width = new GridLength(1, GridUnitType.Star);
                totalSumGrid.ColumnDefinitions.Add(colDef1);
                totalSumGrid.ColumnDefinitions.Add(colDef2);
                totalSumGrid.ColumnDefinitions.Add(colDefin3);

                var totalSumTextBlock = new TextBlock();
                totalSumTextBlock.Text = "Total sum: " + totalSum + " $";
                totalSumTextBlock.FontWeight = FontWeight.FromOpenTypeWeight(5);
                totalSumTextBlock.FontSize = 15;
                Grid.SetColumn(totalSumTextBlock, 2);

                totalSumGrid.Children.Add(totalSumTextBlock);
                ordersStackPanel.Children.Add(totalSumGrid);
                //end drawing total sum grid

                //drawing customer information
                var customerInfoLabelTextBox = new TextBlock
                {
                    Text = "Customer information:",
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                ordersStackPanel.Children.Add(customerInfoLabelTextBox);

                var customerNameLabelTextBox = new TextBlock
                {
                    Text = "Full name: " + order.Customer.FullName,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(customerNameLabelTextBox);

                var customerEmailNameTextBox = new TextBlock
                {
                    Text = "E-mail: " + order.Customer.Email,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(customerEmailNameTextBox);

                var customerPersonalCodeTextBox = new TextBlock
                {
                    Text = "Personal code: " + order.Customer.PersonalCode,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(customerPersonalCodeTextBox);
                //end drawing customer information

                //drawing employee information
                var employeeInfoLabelTextBox = new TextBlock
                {
                    Text = "Responsible employee information:",
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                ordersStackPanel.Children.Add(employeeInfoLabelTextBox);

                var employeeNameLabelTextBox = new TextBlock
                {
                    Text = "Full name: " + order.ResponsibleEmployee.FullName,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(employeeNameLabelTextBox);

                var employeeEmailNameTextBox = new TextBlock
                {
                    Text = "E-mail: " + order.ResponsibleEmployee.Email,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(employeeEmailNameTextBox);

                var employeePersonalCodeTextBox = new TextBlock
                {
                    Text = "Personal code: " + order.ResponsibleEmployee.PersonalCode,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(employeePersonalCodeTextBox);

                var employeeAgreementDate = new TextBlock
                {
                    Text = "Agreement date: " + order.ResponsibleEmployee.AgreementDate,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(employeeAgreementDate);

                var employeeAgreementNumber = new TextBlock
                {
                    Text = "Agreement number: " + order.ResponsibleEmployee.AgreementNr,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5),
                };
                ordersStackPanel.Children.Add(employeeAgreementNumber);
                //end drawing employee information
            }





        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            controlIndex++;
            if (controlIndex == orders.Count) controlIndex = 0;
            PrintOrderInformation(controlIndex);

        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            controlIndex--;
            if (controlIndex == -1) controlIndex = orders.Count - 1;
            PrintOrderInformation(controlIndex);
        }
    }
}

