using System;
using System.Linq;
using UsersDomain.Exceptions;

namespace UsersDomain.ValueTypes
{
    public class Password : IEquatable<Password>
    {
        private const int MinLength = 8;
        private const int MaxLenght = 15;

        private readonly string password;

        public Password(string value)
        {
            password = value;
            Validate();
        }

        private void Validate()
        {
            if (IsEmpty || LengthIsNotInRange || HasNoDigits || HasNoLetters || HasNoSpecial || HasColon)
            {
                throw new PasswordException("Bad password");
            }
        }

        private bool IsEmpty { get => string.IsNullOrEmpty(password); }

        private bool LengthIsNotInRange { get => password.Length < MinLength || password.Length > MaxLenght; }

        private bool HasNoDigits { get => !password.Any(char.IsDigit); }

        private bool HasNoLetters { get => !password.Any(char.IsLetter); }

        private bool HasNoSpecial { get => password.All(char.IsLetterOrDigit); }

        private bool HasColon { get => password.Contains(':'); }

        public override string ToString()
        {
            return password;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Password);
        }

        public override int GetHashCode()
        {
            return password.GetHashCode();
        }

        public bool Equals(Password other)
        {
            return other?.password == password;
        }
    }
}
