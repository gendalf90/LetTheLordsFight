using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Users
{
    public interface IUsers
    {
        Task<UserDto> GetByLoginAsync(string login);

        Task SaveAsync(UserDto data);
    }
}
