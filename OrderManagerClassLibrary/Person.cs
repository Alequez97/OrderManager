using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Key]
        public string PersonalCode
        {
            get => _personalCode;
            set => _personalCode = value;
        }

        [NotMapped]
        public string FullName => _name + " " + _surname;

        public string Email
        {
            get => _email;
            set
            {
                if (IsValidEmail(value)) _email = value;
            }
        }

        public override string ToString()
        {
            return "\tfull name: " + FullName + "\n\tpersonal code: " + PersonalCode + "\n\te-mail: " + Email;
        }

        bool IsValidEmail(string email) //https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
