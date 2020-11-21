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
                personalCodeTextBox.Text = person.PersonalCode;
            }
            else
            {
                nameTextBox.Text = "";
                surnameTextBox.Text = "";
                emailTextBox.Text = "";
                personalCodeTextBox.Text = "";
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
            var personalCode = personalCodeTextBox.Text.Trim();
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
            if (personalCode.Equals(""))
            {
                personalCodeLabel.Content = "Personal code field is required";
                personalCodeLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                personalCodeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputIsEmpty = true;
            }

            if (inputIsEmpty) return;

            Dictionary<int, string> response;

            if (person is Employee)
            {
                response = applicationManager.EditEmployee(name, surname, email, personalCode, oldPersonalCode, selectedIndex); //method return dictionary with response code and message
            }
            else
            {
                response = applicationManager.EditCustomer(name, surname, email, personalCode, oldPersonalCode, selectedIndex);
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
            personalCodeLabel.Content = "";
        }

        private void RestoreTextBoxTextsToDefault()
        {
            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            personalCodeTextBox.Text = "";
            emailTextBox.Text = "";

        }

        private void RestoreTextBoxBordersToDefault()
        {
            nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            surnameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            emailTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            personalCodeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
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
                    personComboBox.Items.Add(employee.FullName);
                }
            }
            else
            {
                Title = "Edit customer";
                foreach (var customer in applicationManager.getCustomers())
                {
                    persons.Add(customer);
                    personComboBox.Items.Add(customer.FullName);
                }
            }

            personComboBox.SelectedIndex = -1;
        }
    }
}
