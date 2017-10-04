using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MapDomain.Exceptions
{
    public class BadLocationException : Exception
    {
        public BadLocationException()
        {
        }

        public BadLocationException(string message) : base(message)
        {
        }

        public BadLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadLocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
