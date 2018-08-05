using ArmiesDomain.Entities;
using System.Threading.Tasks;

namespace ArmiesDomain.Factories.Armies
{
    public interface IArmyFactory
    {
        Task<Army> BuildAsync(ArmyFactoryDto data);
    }
}
