using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Models
{
    [Serializable]
    public class Customer : Person
    {
        public Customer()
        {

        }

        public Customer(string name, string surname, string personalCode, string email)
        {
            Name = name;
            Surname = surname;
            PersonalCode = personalCode;
            Email = email;
        }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
