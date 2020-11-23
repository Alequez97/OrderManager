using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Edit
{
    /// <summary>
    /// Interaction logic for EditPerson.xaml
    /// </summary>
    public partial class EditPerson : Window
    {
        private Person person;
        private List<Person> persons;
        private ApplicationManager applicationManager;

        public EditPerson(Person person)
        {
            this.person = person;
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();
            persons = new List<Person>();

            FillPersonsListAndComboBoxWithNames();
        }

        private void FillTextBoxesWithPersonInformation(object sender, SelectionChangedEventArgs e)
        {
            RestoreErrorLabelsToDefault();
            RestoreTextBoxTextsToDefault();
            RestoreTextBoxBordersToDefault();

            if (personComboBox.SelectedIndex != -1)
            {
                var person = persons[personComboBox.SelectedIndex];
                nameTextBox.Text = person.Name;
                surnameTextBox.Text = person.Surname;
                emailTextBox.Text = person.Email;
            }
            else
            {
                nameTextBox.Text = "";
                surnameTextBox.Text = "";
                emailTextBox.Text = "";
            }

            responseGrid.Children.Clear();
        }

        private void EditPersonButton_OnClick(object sender, RoutedEventArgs e)
        {
            RestoreErrorLabelsToDefault();
            RestoreTextBoxBordersToDefault();

            if (personComboBox.SelectedIndex == -1)
            {
                personComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                personsErrorLabel.Content = person is Employee ? "Please choose employee" : "Please choose customer";
                return;
            }

            var name = nameTextBox.Text.Trim();
            var surname = surnameTextBox.Text.Trim();
            var email = emailTextBox.Text.Trim();
            var oldPersonalCode = persons[personComboBox.SelectedIndex].PersonalCode;
            var selectedIndex = personComboBox.SelectedIndex;

            responseGrid.Children.Clear();

            bool inputIsEmpty = false;
            if (name.Equals(""))
            {
                nameLabel.Content = "Name field is required";
                nameLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputIsEmpty = true;
            }
            if (surname.Equals(""))
            {
                surnameLabel.Content = "Surname field is required";
                surnameLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                surnameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputIsEmpty = true;
            }
            if (email.Equals(""))
            {
                emailLabel.Content = "E-mail field is required";
                emailLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                emailTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputIsEmpty = true;
            }

            if (inputIsEmpty) return;

            Dictionary<int, string> response;

            if (person is Employee)
            {
                response = applicationManager.EditEmployee(name, surname, email, oldPersonalCode, selectedIndex); //method return dictionary with response code and message
            }
            else
            {
                response = applicationManager.EditCustomer(name, surname, email, oldPersonalCode, selectedIndex);
            }

            //1 - success, 0 - error, -1 - warning
            var responseCode = response.Keys.ElementAt(0);
            SolidColorBrush color;
            if (responseCode == 1)
            {
                color = new SolidColorBrush(Color.FromRgb(0, 140, 0));
                personComboBox.SelectedIndex = -1;
                personComboBox.Items[selectedIndex] = name + " " + surname;
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

            if (responseCode == 1) FillPersonsListAndComboBoxWithNames();
        }

        private void RestoreErrorLabelsToDefault()
        {

            personsErrorLabel.Content = "";
            nameLabel.Content = "";
            surnameLabel.Content = "";
            emailLabel.Content = "";
        }

        private void RestoreTextBoxTextsToDefault()
        {
            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            emailTextBox.Text = "";

        }

        private void RestoreTextBoxBordersToDefault()
        {
            nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            surnameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            emailTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            personComboBoxBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void FillPersonsListAndComboBoxWithNames()
        {
            persons.Clear();
            personComboBox.Items.Clear();

            if (person is Employee)
            {
                Title = "Edit employee";
                foreach (var employee in applicationManager.getEmployees())
                {
                    persons.Add(employee);
                    personComboBox.Items.Add(employee.FullName + " (" + employee.PersonalCode + ")");
                }
            }
            else
            {
                Title = "Edit customer";
                foreach (var customer in applicationManager.getCustomers())
                {
                    persons.Add(customer);
                    personComboBox.Items.Add(customer.FullName + " (" + customer.PersonalCode + ")");
                }
            }

            personComboBox.SelectedIndex = -1;
        }
    }
}
