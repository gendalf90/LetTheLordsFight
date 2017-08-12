using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Exceptions
{
    public class NoPermissionException : Exception
    {
        public NoPermissionException()
        {
        }

        public NoPermissionException(string message) : base(message)
        {
        }

        public NoPermissionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
