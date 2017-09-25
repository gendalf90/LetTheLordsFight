using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StorageDomain.Exceptions
{
    public class NotAuthorizedException : ValidationException
    {
        public NotAuthorizedException()
        {
        }

        public NotAuthorizedException(string message) : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
