using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Models
{
    [Serializable]
    public class Product
    {

        public Product()
        {

        }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Price}$";
        }

    }
}
