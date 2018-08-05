using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Armors
{
    public interface IArmors
    {
        Task<ArmorRepositoryDto> GetByNameAsync(string name);
    }
}
