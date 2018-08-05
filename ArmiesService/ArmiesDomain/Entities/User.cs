using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Users;
using ArmiesDomain.ValueObjects;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class User
    {
        private static readonly Cost defaultArmyLimit = new Cost(50);

        private Cost armyLimit;

        private User(string login)
        {
            Login = login;
        }

        public bool IsArmyCostLimitExceeded(Cost armyCost)
        {
            return armyCost.IsGreaterThan(armyLimit);
        }

        public string Login { get; private set; }

        public async Task SaveAsync(IUsers repository)
        {
            var data = new UserRepositoryDto();
            data.Login = Login;
            armyLimit.FillUserData(data);
            await repository.SaveAsync(data);
        }

        public static async Task<User> LoadByLoginAsync(IUsers repository, string login)
        {
            var data = await repository.GetByLoginAsync(login);

            return new User(data.Login)
            {
                armyLimit = new Cost(data.ArmyCostLimit)
            };
        }

        public static User CreateWithLogin(string login)
        {
            return new User(login)
            {
                armyLimit = defaultArmyLimit
            };
        }
    }
}
