using System;
using System.Runtime.Serialization;

namespace ArmiesService.Queries.Army
{
    public class ArmyNotFoundException : Exception
    {
        public ArmyNotFoundException() : base("Army not found")
        {
        }

        public ArmyNotFoundException(string ownerLogin) : base($"Army for login {ownerLogin} not found")
        {
        }

        protected ArmyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
