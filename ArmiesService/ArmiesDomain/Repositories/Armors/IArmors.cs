using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Armors
{
    public interface IArmors
    {
        Task<ArmorDto> GetByNameAsync(string name);
    }
}
