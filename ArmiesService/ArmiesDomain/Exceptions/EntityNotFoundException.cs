using System;
using System.Runtime.Serialization;

namespace ArmiesDomain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("Entity not found")
        {
        }

        protected EntityNotFoundException(string key, string name) : base($"'{name}' by '{key}' not found")
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static EntityNotFoundException CreateUser(string key)
        {
            return new EntityNotFoundException(key, "User");
        }

        public static EntityNotFoundException CreateSquad(string key)
        {
            return new EntityNotFoundException(key, "Squad");
        }

        public static EntityNotFoundException CreateWeapon(string key)
        {
            return new EntityNotFoundException(key, "Weapon");
        }

        public static EntityNotFoundException CreateArmor(string key)
        {
            return new EntityNotFoundException(key, "Armor");
        }
    }
}
