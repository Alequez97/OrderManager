using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary
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


        public DateTime AgreementDate { get; }
        public string AgreementNr { get; }

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
