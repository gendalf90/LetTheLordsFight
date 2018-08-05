using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Users
{
    public interface IUsers
    {
        Task<UserRepositoryDto> GetByLoginAsync(string login);

        Task SaveAsync(UserRepositoryDto data);
    }
}
