using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class SquadException : ArgumentException
    {
        public SquadException()
        {
        }

        public SquadException(string message) : base(message)
        {
        }

        public SquadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SquadException(string message, string paramName) : base(message, paramName)
        {
        }

        public SquadException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }

        protected SquadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
