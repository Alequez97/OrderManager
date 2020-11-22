using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagerClassLibrary;

namespace TestDbSaving
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var cxt = new OrderManagerDbContext())
            {
                var customer = new Customer()
                {
                    Name = "Jurijs",
                    Surname = "Sluncevs",
                    Email = "juras@gmail.com",
                    PersonalCode = "121313-31313"
                };

                cxt.Customers.Add(customer);
                cxt.SaveChanges();
            }

        }
    }
}
