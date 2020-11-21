using System;
using System.Net.Mail;

namespace OrderManagerClassLibrary
{
    [Serializable]
    public abstract class Person
    {
        private string _name;
        private string _surname;
        private string _personalCode;
        private string _email;

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Surname
        {
            get => _surname;
            set => _surname = value;
        }
        public string PersonalCode
        {
            get => _personalCode;
            set => _personalCode = value;
        }

        public string FullName => _name + " " + _surname;

        public string Email
        {
            get => _email;
            set
            {
                try
                {
                    var address = new MailAddress(value);
                    var isValid = (address.Address == value);
                    if (isValid)
                    {
                        _email = value;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Email address is invalid");
                }
            }
        }
        public override string ToString()
        {
            return "\tfull name: " + FullName + "\n\tpersonal code: " + PersonalCode + "\n\te-mail: " + Email;
        }
    }
}
