using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UsersDomain.Exceptions
{
    public class UserTypeException : Exception
    {
        public UserTypeException()
        {
        }

        public UserTypeException(string message) : base(message)
        {
        }

        public UserTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
