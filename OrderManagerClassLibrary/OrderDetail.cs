using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary
{
    [Serializable]
    public class OrderDetail
    {

        public int Id { get; set; }

        public Product Product { get; set; }

        public int Amount { get; set; }

        public override string ToString()
        {

            return "product name: " + Product.Name + ", product price: " + string.Format("{0:0.00}", Product.Price) + "$, amount: " + Amount;
        }
    }
}
