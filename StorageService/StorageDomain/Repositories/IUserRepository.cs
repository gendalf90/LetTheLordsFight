using StorageDomain.Entities;
using System.Threading.Tasks;

namespace StorageDomain.Repositories
{
    public interface IUsersRepository
    {
        User GetCurrent();
    }
}
