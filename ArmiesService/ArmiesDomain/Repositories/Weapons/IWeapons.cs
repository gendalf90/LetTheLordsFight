using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Weapons
{
    public interface IWeapons
    {
        Task<WeaponDto> GetByNameAsync(string name);
    }
}
