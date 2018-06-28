using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class ArmyException : ArgumentException
    {
        public ArmyException()
        {
        }

        public ArmyException(string message) : base(message)
        {
        }

        public ArmyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ArmyException(string message, string paramName) : base(message, paramName)
        {
        }

        public ArmyException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }

        protected ArmyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
