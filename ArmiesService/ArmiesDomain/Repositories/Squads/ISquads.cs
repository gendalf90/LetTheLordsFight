using System.Threading.Tasks;

namespace ArmiesDomain.Repositories.Squads
{
    public interface ISquads
    {
        Task<SquadRepositoryDto> GetByTypeAsync(string type);
    }
}
