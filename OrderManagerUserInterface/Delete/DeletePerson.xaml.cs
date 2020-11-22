using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Delete
{
    /// <summary>
    /// Interaction logic for DeletePerson.xaml
    /// </summary>
    public partial class DeletePerson : Window
    {

        private Person person;
        private List<Person> persons;
        private ApplicationManager applicationManager;

        public DeletePerson(Person person)
        {
            InitializeComponent();

            this.person = person;
            applicationManager = ApplicationManager.Instance();
            persons = new List<Person>();

            FillPersonsListAndComboBoxWithNames();
        }


        private void DeletePersonButton_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteIndex = personComboBox.SelectedIndex;
            responseGrid.Children.Clear();

            if (deleteIndex == -1)
            {
                personComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                personsErrorLabel.Content = person is Employee ? "Please choose employee" : "Please choose customer";
                return;
            }

            Dictionary<int, string> response;
            if (person is Employee)
            {
                response = applicationManager.DeleteEmployee(persons.ElementAt(deleteIndex) as Employee);
            }
            else
            {
                response = applicationManager.DeleteCustomer(persons.ElementAt(deleteIndex) as Customer);
            }

            FillPersonsListAndComboBoxWithNames();

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

        private void FillPersonsListAndComboBoxWithNames()
        {
            persons.Clear();
            personComboBox.Items.Clear();
            responseGrid.Children.Clear();

            if (person is Employee)
            {
                Title = "Delete employee";
                foreach (var employee in applicationManager.getEmployees())
                {
                    persons.Add(employee);
                    personComboBox.Items.Add(employee.FullName);
                }
            }
            else
            {
                Title = "Delete customer";
                foreach (var customer in applicationManager.getCustomers())
                {
                    persons.Add(customer);
                    personComboBox.Items.Add(customer.FullName);
                }
            }

        }

        private void PersonComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            personComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            personsErrorLabel.Content = "";
        }
    }
}
