using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class SquadException : Exception
    {
        protected SquadException(string message) : base(message)
        {
        }

        protected SquadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool IsType { get; private set; }

        public bool IsQuantity { get; private set; }

        public static SquadException CreateType()
        {
            return new SquadException("Squad type is empty")
            {
                IsType = true
            };
        }

        public static SquadException CreateQuantity()
        {
            return new SquadException("Quantity of squad must be greater than 0")
            {
                IsQuantity = true
            };
        }
    }
}
