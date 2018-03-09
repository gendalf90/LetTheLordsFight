using System.Threading.Tasks;

namespace UsersDomain.Repositories
{
    public interface IUsers
    {
        Task SaveAsync(UserDto dto); //unique index on login field
    }
}
