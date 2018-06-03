using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Armies
{
    public interface IArmies
    {
        Task SaveAsync(ArmyDto data);
    }
}
