using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Show
{
    /// <summary>
    /// Interaction logic for ShowEmployees.xaml
    /// </summary>
    public partial class ShowEmployees : Window
    {

        private List<Employee> employees;

        public ShowEmployees(List<Employee> employees)
        {
            InitializeComponent();
            this.employees = employees;
            PrintInformation();
        }

        public ShowEmployees()
        {
            InitializeComponent();
            PrintInformation();
        }

        private void PrintInformation()
        {
            if (employees.Count == 0 || employees == null)
            {
                Label label = new Label();
                label.Content = "No employee added yet!";
                label.FontSize = 30;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                myStackPanel.Children.Add(label);
            }
            else
            {
                List<Label> mainLabels = new List<Label>();
                Grid grid = new Grid();                                                         //https://stackoverflow.com/questions/9803710/programmatically-setting-the-width-of-a-grid-column-with-in-wpf
                grid.Background = new SolidColorBrush(Color.FromRgb(200, 150, 200));

                for (int i = 0; i < 6; i++)
                {
                    var colDef = new ColumnDefinition();
                    colDef.Width = new GridLength(1, GridUnitType.Star);
                    grid.ColumnDefinitions.Add(colDef);

                }

                RowDefinition rowDef1 = new RowDefinition();
                rowDef1.Height = new GridLength(50);
                grid.RowDefinitions.Add(rowDef1);

                var label = new Label();    //define container variable for textBoxes
                label.Content = "Name";
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label);         //add label to list

                label = new Label();
                label.Content = "Surname";
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label);         //add label to list

                label = new Label();
                label.Content = "E-mail";
                Grid.SetColumn(label, 2);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label);         //add label to list

                label = new Label();
                label.Content = "Personal Code";
                Grid.SetColumn(label, 3);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label);         //add label to list

                label = new Label();
                label.Content = "Agreement Date";
                Grid.SetColumn(label, 4);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label);         //add label to list

                label = new Label();
                label.Content = "Agreement Number";
                Grid.SetColumn(label, 5);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
                mainLabels.Add(label);         //add label to list

                foreach (var mainLabel in mainLabels)
                {
                    mainLabel.VerticalAlignment = VerticalAlignment.Center;
                    mainLabel.HorizontalAlignment = HorizontalAlignment.Center;
                }

                myStackPanel.Children.Add(grid);

                int rowIndex = 1;
                List<TextBox> newTextBoxes;
                foreach (var employee in employees)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = new GridLength(30);
                    grid.RowDefinitions.Add(rowDef);

                    newTextBoxes = new List<TextBox>();

                    newTextBoxes.Add(new TextBox() { Text = employee.Name });
                    newTextBoxes.Add(new TextBox() { Text = employee.Surname });
                    newTextBoxes.Add(new TextBox() { Text = employee.Email });
                    newTextBoxes.Add(new TextBox() { Text = employee.PersonalCode });
                    newTextBoxes.Add(new TextBox() { Text = employee.AgreementDate.ToString() });
                    newTextBoxes.Add(new TextBox() { Text = employee.AgreementNr });

                    Grid.SetColumn(newTextBoxes[0], 0);
                    Grid.SetRow(newTextBoxes[0], rowIndex);

                    Grid.SetColumn(newTextBoxes[1], 1);
                    Grid.SetRow(newTextBoxes[1], rowIndex);

                    Grid.SetColumn(newTextBoxes[2], 2);
                    Grid.SetRow(newTextBoxes[2], rowIndex);

                    Grid.SetColumn(newTextBoxes[3], 3);
                    Grid.SetRow(newTextBoxes[3], rowIndex);

                    Grid.SetColumn(newTextBoxes[4], 4);
                    Grid.SetRow(newTextBoxes[4], rowIndex);

                    Grid.SetColumn(newTextBoxes[5], 5);
                    Grid.SetRow(newTextBoxes[5], rowIndex);

                    grid.Children.Add(newTextBoxes[0]);
                    grid.Children.Add(newTextBoxes[1]);
                    grid.Children.Add(newTextBoxes[2]);
                    grid.Children.Add(newTextBoxes[3]);
                    grid.Children.Add(newTextBoxes[4]);
                    grid.Children.Add(newTextBoxes[5]);

                    //foreach (var newTextBox in newTextBoxes)
                    //{
                    //    newTextBox.VerticalAlignment = VerticalAlignment.Center;
                    //    newTextBox.HorizontalAlignment = HorizontalAlignment.Center;
                    //}

                    rowIndex++;
                }
            }


        }
    }
}
