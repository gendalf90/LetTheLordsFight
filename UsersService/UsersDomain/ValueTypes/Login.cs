using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using UsersDomain.Exceptions;

namespace UsersDomain.ValueTypes
{
    public class Login : IEquatable<Login>
    {
        private MailAddress mailAddress;

        public Login(string value)
        {
            TryCreateMailAddress(value);
        }

        private void TryCreateMailAddress(string address)
        {
            try
            {
                mailAddress = new MailAddress(address);
            }
            catch
            {
                throw new LoginInvalidException();
            }
        }

        public override string ToString()
        {
            return mailAddress.Address;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Login);
        }

        public override int GetHashCode()
        {
            return mailAddress.Address.GetHashCode();
        }

        public bool Equals(Login other)
        {
            return mailAddress.Address.ToUpper() == other?.mailAddress?.Address?.ToUpper();
        }
    }
}
