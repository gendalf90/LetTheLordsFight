using ArmiesDomain.Entities;

namespace ArmiesDomain.Factories.Armies
{
    public interface IFactory
    {
        Army Build(ArmyData data);
    }
}
