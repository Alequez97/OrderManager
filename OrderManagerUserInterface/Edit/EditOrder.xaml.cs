using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManagerClassLibrary;

namespace OrderManagerUserInterface.Edit
{
    /// <summary>
    /// Interaction logic for EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {

        private ApplicationManager applicationManager;
        private List<Employee> employees;
        private List<Order> orders;

        public EditOrder()
        {
            InitializeComponent();
            applicationManager = ApplicationManager.Instance();
            employees = new List<Employee>();
            orders = new List<Order>();

            FillComboBoxes();
        }



        private void EditOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            var orderSelectedIndex = orderNumberComboBox.SelectedIndex;

            if (orderSelectedIndex == -1)
            {
                orderNumberComboBoxBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                orderNumberErrorTextBlock.Text = "Please choose order you want to edit";
                return;
            }

            var employeeSelectedIndex = employeeComboBox.SelectedIndex;
            var stateSelectedIndex = stateComboBox.SelectedIndex;

            var response = applicationManager.EditOrder(employees[employeeSelectedIndex], (State)stateSelectedIndex, orderSelectedIndex);
            var responseCode = response.Keys.ElementAt(0);

            if (responseCode == 1)
            {
                responseLabel.Content = response[responseCode];
                responseLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
            }
        }


        private void FillOrderInformation(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = orderNumberComboBox.SelectedIndex;
            var employee = orders[selectedIndex].ResponsibleEmployee;
            var responsibleEmployeeIndex = GetEmployeeIndexInCollection(employee);

            employeeComboBox.SelectedIndex = responsibleEmployeeIndex;
            stateComboBox.SelectedItem = orders[selectedIndex].State;
        }

        private int GetEmployeeIndexInCollection(Employee employee)
        {
            int index = 0;
            foreach (var e in employees)
            {
                if (e.PersonalCode.Equals(employee.PersonalCode)) return index;
                index++;
            }

            return -1;
        }

        private void FillComboBoxes()
        {
            employees.Clear();
            orders.Clear();
            orderNumberComboBox.Items.Clear();
            employeeComboBox.Items.Clear();
            stateComboBox.Items.Clear();

            employees = applicationManager.getEmployees();
            orders = applicationManager.getOrders();

            foreach (var order in orders)
            {
                orderNumberComboBox.Items.Add(order.Number);
            }

            foreach (var employee in employees)
            {
                employeeComboBox.Items.Add(employee.FullName + " (p.c. - " + employee.PersonalCode + ")");
            }

            foreach (var state in Enum.GetValues(typeof(State)))
            {
                stateComboBox.Items.Add(state);
            }
        }
    }
}
