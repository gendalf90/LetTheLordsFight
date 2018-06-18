using ArmiesDomain.Entities;
using System.Threading.Tasks;

namespace ArmiesDomain.Factories.Armies
{
    public interface IFactory
    {
        Task<Army> BuildAsync(ArmyData data);
    }
}
