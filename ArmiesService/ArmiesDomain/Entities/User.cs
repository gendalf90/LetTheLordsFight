using ArmiesDomain.Repositories.Users;
using ArmiesDomain.ValueObjects;
using System;

namespace ArmiesDomain.Entities
{
    public class User
    {
        private Cost armyCostLimit;

        public bool IsArmyCostLimitExceeded(Cost armyCost)
        {
            return armyCost.IsGreaterThan(armyCostLimit);
        }

        public string Login { get; private set; }

        //здесь также хранится опыт
        //методы для повышения лимита стоимости

        public void Save(IUsers repository)
        {
            throw new NotImplementedException();
        }

        public static User LoadByLogin(IUsers repository, string login)
        {
            throw new NotImplementedException();
        }
    }
}
