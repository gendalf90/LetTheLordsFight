using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UsersDomain.Exceptions
{
    public class PasswordInvalidException : Exception
    {
        public PasswordInvalidException()
        {
        }

        public PasswordInvalidException(string message) : base(message)
        {
        }

        public PasswordInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PasswordInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
