using System;
using UsersDomain.Exceptions;
using System.Text.RegularExpressions;

namespace UsersDomain.ValueTypes
{
    public class Login : IEquatable<Login>
    {
        private const int MaxLength = 256;

        private static readonly Regex pattern = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly string address;

        public Login(string value)
        {
            address = value;
            Validate();
        }

        private void Validate()
        {
            if (IsNullOrEmpty || IsTooLong || IsNotMatchThePattern)
            {
                throw new LoginException("Bad login");
            }
        }

        private bool IsNullOrEmpty { get => string.IsNullOrEmpty(address); }

        private bool IsTooLong { get => address.Length > MaxLength; }

        private bool IsNotMatchThePattern { get => !pattern.IsMatch(address); }

        public override string ToString()
        {
            return address;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Login);
        }

        public override int GetHashCode()
        {
            return address.GetHashCode();
        }

        public bool Equals(Login other)
        {
            return address.Equals(other?.address, StringComparison.OrdinalIgnoreCase);
        }
    }
}
