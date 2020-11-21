using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary
{
    [Serializable]
    public class Order
    {

        private DateTime _orderDate;
        private List<OrderDetail> _details;

        public Order()
        {
            _orderDate = DateTime.Now;
            Number = RandomStringGenerator(10);
            _details = new List<OrderDetail>();
            State = State.New;
        }

        public Order(Customer customer, Employee responsibleEmployee, DateTime dateTime)
        {
            Customer = customer;
            ResponsibleEmployee = responsibleEmployee;
            _orderDate = dateTime;
            Number = RandomStringGenerator(10);
            State = State.New;
            _details = new List<OrderDetail>();
        }

        public string Number { get; private set; }

        public DateTime OrderDate
        {
            get => _orderDate;
            set => _orderDate = value;
        }

        public State State { get; set; }

        public Customer Customer { get; set; }

        public Employee ResponsibleEmployee { get; set; }

        public List<OrderDetail> Details
        {
            get => _details;
            set => _details = value;
        }

        public void addProduct(string name, int amount)
        {

            var check = false;
            foreach (var detail in Details)
            {
                if (detail.Product.Name.Equals(name))
                {
                    detail.Amount += amount;
                    check = true;
                }
            }

            if (!check)
            {


                ApplicationManager applicationManager = ApplicationManager.Instance();

                bool containsProduct = false;
                foreach (Product product in applicationManager.getProducts())
                {
                    if (product.Name.Equals(name))
                    {
                        OrderDetail newOrderDetail = new OrderDetail();
                        newOrderDetail.Product = product;
                        newOrderDetail.Amount = amount;
                        Details.Add(newOrderDetail);
                        containsProduct = true;
                    }
                }

                if (!containsProduct)
                {
                    Console.WriteLine("Sorry, there are no this sort of product");
                }
            }



        }

        public override string ToString()
        {
            return $"****************************************************************************\n" +
                $"Order number: {Number}, Order date: {OrderDate}, Order state: {State}" +
                $"\nOrder details:\n" + orderDetailsStringCreator() +
                $"\nCustomer information: \n{Customer.ToString()}" +
                $"\nResponsible employee information: \n{ResponsibleEmployee.ToString()}" +
                $"\n****************************************************************************";
        }

        private string orderDetailsStringCreator()
        {
            string allDetailsInCurrentOrder = "";
            int index = 0;
            decimal totalSum = 0;
            foreach (OrderDetail detail in Details)
            {
                index++;
                allDetailsInCurrentOrder += "\tDetail No " + index + ": " + detail.ToString() + "\n";
                totalSum += detail.Amount * detail.Product.Price;
            }

            allDetailsInCurrentOrder += $"\tTotal sum: {string.Format("{0:0.00}", totalSum)}$\n";

            return allDetailsInCurrentOrder;
        }

        private string RandomStringGenerator(int symbolsCount)
        {
            string generatedString = "";
            var r = new Random();
            for (int i = 0; i < symbolsCount; i++)
            {
                int rInt = r.Next(48, 58);
                char c = (char)rInt;
                generatedString += c.ToString();
            }

            return generatedString;
        }

    }
}
