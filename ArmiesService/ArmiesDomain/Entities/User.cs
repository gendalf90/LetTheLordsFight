using ArmiesDomain.Repositories.Users;
using ArmiesDomain.ValueObjects;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class User
    {
        private int armyCostLimit;

        private User(string login, int armyCostLimit)
        {
            Login = login;
            this.armyCostLimit = armyCostLimit;
        }

        public bool IsArmyCostLimitExceeded(Cost armyCost)
        {
            var currentArmyCostLimit = new Cost(armyCostLimit);
            return armyCost.IsGreaterThan(currentArmyCostLimit);
        }

        public string Login { get; private set; }

        //здесь также хранится опыт
        //методы для повышения лимита стоимости

        public async Task SaveAsync(IUsers repository)
        {
            var data = new UserDto
            {
                Login = Login,
                ArmyCostLimit = armyCostLimit
            };

            await repository.SaveAsync(data);
        }

        public static async Task<User> LoadByLoginAsync(IUsers repository, string login)
        {
            var data = await repository.GetByLoginAsync(login);
            return new User(data.Login, data.ArmyCostLimit);
        }
    }
}
