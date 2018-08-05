using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Weapons
{
    public interface IWeapons
    {
        Task<WeaponRepositoryDto> GetByNameAsync(string name);
    }
}
