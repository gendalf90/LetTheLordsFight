using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class QuantityException : Exception
    {
        public QuantityException() : base("Quantity must be greater than or equal to 0")
        {
        }

        protected QuantityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
