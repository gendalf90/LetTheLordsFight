using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class ArmyException : Exception
    {
        protected ArmyException(string message) : base(message)
        {
        }

        protected ArmyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool IsCost { get; private set; }

        public bool IsSquads { get; private set; }

        public bool IsOwner { get; private set; }

        public static ArmyException CreateCostLimitExceeded()
        {
            return new ArmyException("The army is too expensive")
            {
                IsCost = true
            };
        }

        public static ArmyException CreateNoSquads()
        {
            return new ArmyException("Squads list is empty")
            {
                IsSquads = true
            };
        }

        public static ArmyException CreateEmptyOwner()
        {
            return new ArmyException("Owner must be set")
            {
                IsOwner = true
            };
        }
    }
}
