using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UsersDomain.Exceptions
{
    public class LoginInvalidException : Exception
    {
        public LoginInvalidException()
        {
        }

        public LoginInvalidException(string message) : base(message)
        {
        }

        public LoginInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
