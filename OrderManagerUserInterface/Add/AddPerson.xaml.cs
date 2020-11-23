using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface
{
    /// <summary>
    /// Interaction logic for AddPerson.xaml
    /// </summary>
    public partial class AddPerson : Window
    {
        private ApplicationManager applicationManager;
        private Person person;

        public AddPerson(Employee person)
        {
            this.person = person;
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();
        }

        public AddPerson(Customer person)
        {
            this.person = person;
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();
        }

        private void AddPersonButton_OnClick(object sender, RoutedEventArgs e)
        {
            var name = nameTextBox.Text.Trim();
            var surname = surnameTextBox.Text.Trim();
            var email = emailTextBox.Text.Trim();
            var personalCode = personalCodeTextBox.Text.Trim();

            setLabelsEmpty();
            SetDefaultBorderColorForTextBoxes();
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
            if (!new Regex(@"^[0-9]{6}-[0-9]{5}\z").IsMatch(personalCode))
            {
                personalCodeLabel.Text = "Personal code must contain numbers\nand match pattern xxxxxx-xxxxx";
                personalCodeLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                personalCodeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputIsEmpty = true;
            }
            if (personalCode.Equals(""))
            {
                personalCodeLabel.Text = "Personal code field is required";
                personalCodeLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                personalCodeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                inputIsEmpty = true;
            }

            if (inputIsEmpty) return;

            Dictionary<int, string> saveResponse;
            if (person is Employee)
            {
                saveResponse = applicationManager.AddEmployee(name, surname, personalCode, email);
            }
            else
            {
                saveResponse = applicationManager.AddCustomer(name, surname, personalCode, email);
            }

            int responseCode = saveResponse.Keys.ElementAt(0);
            string responseMessage = saveResponse[responseCode];

            if (responseCode == 0)
            {
                responseGrid.Children.Clear();
                Label label = new Label();
                label.Content = "Error! " + responseMessage;
                responseGrid.Children.Add(label);
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                responseGrid.HorizontalAlignment = HorizontalAlignment.Center;

            }
            else
            {
                responseGrid.Children.Clear();
                Label label = new Label();
                label.Content = responseMessage;
                responseGrid.Children.Add(label);
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));

                responseGrid.HorizontalAlignment = HorizontalAlignment.Center;

                EmptyTextBoxes();
            }


        }

        private void SetDefaultBorderColorForTextBoxes()
        {
            nameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            surnameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            emailTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            personalCodeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        }

        private void setLabelsEmpty()
        {
            nameLabel.Content = "";
            surnameLabel.Content = "";
            emailLabel.Content = "";
            personalCodeLabel.Text = "";
        }

        private void EmptyTextBoxes()
        {
            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            personalCodeTextBox.Text = "";
            emailTextBox.Text = "";
        }
    }


}
