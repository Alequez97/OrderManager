using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Models
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
            State = StateEnum.New;
        }

        public Order(Customer customer, Employee responsibleEmployee, DateTime dateTime)
        {
            Customer = customer;
            ResponsibleEmployee = responsibleEmployee;
            _orderDate = dateTime;
            Number = RandomStringGenerator(10);
            State = StateEnum.New;
            _details = new List<OrderDetail>();
        }

        [Key]
        public string Number { get; private set; }

        public DateTime OrderDate
        {
            get => _orderDate;
            set => _orderDate = value;
        }

        public StateEnum State { get; set; }

        public Customer Customer { get; set; }

        public Employee ResponsibleEmployee { get; set; }

        public ICollection<OrderDetail> OrderDetails
        {
            get => _details;
            set => _details = (List<OrderDetail>) value;
        }

        public void AddProduct(string name, int amount)
        {

            var check = false;
            foreach (var detail in OrderDetails)
            {
                if (detail.Product.Name.Equals(name))
                {
                    detail.Amount += amount;
                    check = true;
                }
            }

            if (!check)
            {


                var applicationManager = ApplicationManager.Instance();

                bool containsProduct = false;
                foreach (Product product in applicationManager.getProducts())
                {
                    if (product.Name.Equals(name))
                    {
                        OrderDetail newOrderDetail = new OrderDetail();
                        newOrderDetail.Product = product;
                        newOrderDetail.Amount = amount;
                        OrderDetails.Add(newOrderDetail);
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
                $"\nOrder details:\n" + OrderDetailsStringCreator() +
                $"\nCustomer information: \n{Customer.ToString()}" +
                $"\nResponsible employee information: \n{ResponsibleEmployee.ToString()}" +
                $"\n****************************************************************************";
        }

        private string OrderDetailsStringCreator()
        {
            string allDetailsInCurrentOrder = "";
            int index = 0;
            decimal totalSum = 0;
            foreach (OrderDetail detail in OrderDetails)
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
