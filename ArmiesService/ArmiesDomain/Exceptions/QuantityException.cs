using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class QuantityException : Exception
    {
        public QuantityException()
        {
        }

        public QuantityException(string message) : base(message)
        {
        }

        public QuantityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QuantityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
