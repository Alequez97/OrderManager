using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Models
{
    [Serializable]
    public class Employee : Person
    {
        public Employee()
        {

        }
        public Employee(string name, string surname, string personalCode, string email)
        {

            Name = name;
            Surname = surname;
            PersonalCode = personalCode;
            Email = email;
            AgreementDate = DateTime.Now;
            AgreementNr = AgreementNumberGenerator();

        }

        public DateTime AgreementDate { get; set; }
        public string AgreementNr { get; set; }

        public ICollection<Order> Orders { get; set; }


        public override string ToString()
        {
            return base.ToString() + "\n\tAgreement Date: " + AgreementDate + "\n\tAgreement NR: " + AgreementNr;
        }

        private static string AgreementNumberGenerator()
        {
            Random random = new Random();
            return random.Next(1000, 100000).ToString();
        }

    }
}
