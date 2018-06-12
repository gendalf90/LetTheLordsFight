using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Squads
{
    public interface ISquads
    {
        Task<SquadDto> GetByTypeAsync(string type);
    }
}
