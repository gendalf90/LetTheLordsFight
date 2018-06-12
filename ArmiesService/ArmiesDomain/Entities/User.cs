using ArmiesDomain.Repositories.Users;
using ArmiesDomain.ValueObjects;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class User
    {
        private Cost armyLimit;

        private User()
        {
        }

        public bool IsArmyCostLimitExceeded(Cost armyCost)
        {
            return armyCost.IsGreaterThan(armyLimit);
        }

        public string Login { get; private set; }

        public async Task SaveAsync(IUsers repository)
        {
            var data = new UserDto();
            data.Login = Login;
            armyLimit.FillUserData(data);
            await repository.SaveAsync(data);
        }

        public static async Task<User> LoadByLoginAsync(IUsers repository, string login)
        {
            var data = await repository.GetByLoginAsync(login);

            return new User
            {
                Login = data.Login,
                armyLimit = new Cost(data.ArmyCostLimit)
            };
        }
    }
}
