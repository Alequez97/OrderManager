using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Show
{
    /// <summary>
    /// Interaction logic for ShowProducts.xaml
    /// </summary>
    public partial class ShowProducts : Window
    {

        private List<Product> products;

        public ShowProducts(List<Product> products)
        {
            this.products = products;
            InitializeComponent();
            PrintInformation();
        }

        private void PrintInformation()
        {
            if (products.Count == 0 || products == null)
            {
                Label label = new Label();
                label.Content = "No products added yet!";
                label.FontSize = 30;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                myStackPanel.Children.Add(label);
            }
            else
            {
                List<Label> mainLabels = new List<Label>();
                Grid grid = new Grid();
                grid.Background = new SolidColorBrush(Color.FromRgb(200, 150, 200));

                for (int i = 0; i < 2; i++)
                {
                    var colDef = new ColumnDefinition();
                    colDef.Width = new GridLength(1, GridUnitType.Star);
                    grid.ColumnDefinitions.Add(colDef);

                }

                RowDefinition rowDef1 = new RowDefinition();
                rowDef1.Height = new GridLength(50);
                grid.RowDefinitions.Add(rowDef1);

                var label = new Label(); //define container variable for textBoxes
                label.Content = "Name";
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label); //add label to list

                label = new Label();
                label.Content = "Price";
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label); //add label to list

                foreach (var mainLabel in mainLabels)
                {
                    mainLabel.VerticalAlignment = VerticalAlignment.Center;
                    mainLabel.HorizontalAlignment = HorizontalAlignment.Center;
                }

                myStackPanel.Children.Add(grid);

                int rowIndex = 1;
                List<TextBox> newTextBoxes;
                foreach (var product in products)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = new GridLength(30);
                    grid.RowDefinitions.Add(rowDef);

                    newTextBoxes = new List<TextBox>();

                    newTextBoxes.Add(new TextBox() { Text = product.Name });
                    newTextBoxes.Add(new TextBox() { Text = product.Price + " $" });


                    Grid.SetColumn(newTextBoxes[0], 0);
                    Grid.SetRow(newTextBoxes[0], rowIndex);

                    Grid.SetColumn(newTextBoxes[1], 1);
                    Grid.SetRow(newTextBoxes[1], rowIndex);

                    grid.Children.Add(newTextBoxes[0]);
                    grid.Children.Add(newTextBoxes[1]);

                    rowIndex++;
                }
            }
        }
    }
}
