using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Services;
using ArmiesDomain.Services.ArmyNotifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Army
    {
        private List<Squad> squads;

        public Army(string ownerLogin, IEnumerable<Squad> squads)
        {
            if(string.IsNullOrEmpty(ownerLogin))
            {
                throw ArmyException.CreateEmptyOwner();
            }

            if(squads == null || !squads.Any())
            {
                throw ArmyException.CreateNoSquads();
            }

            OwnerLogin = ownerLogin;
            this.squads = squads.ToList();
        }

        public string OwnerLogin { get; private set; }

        public void CheckCostLimit(IArmyCostLimitService service)
        {
            foreach(var squad in squads)
            {
                squad.CheckCostLimit(service);
            }
        }

        public async Task NotifyThatCreatedAsync(IArmyNotificationService service)
        {
            var data = new ArmyNotificationDto
            {
                Squads = new List<SquadNotificationDto>()
            };

            data.OwnerLogin = OwnerLogin;
            squads.ForEach(squad => squad.FillArmyData(data));
            await service.NotifyThatCreatedAsync(data);
        }

        public async Task SaveAsync(IArmies repository)
        {
            var data = new ArmyRepositoryDto
            {
                Squads = new List<SquadRepositoryDto>()
            };

            data.OwnerLogin = OwnerLogin;
            squads.ForEach(squad => squad.FillArmyData(data));
            await repository.SaveAsync(data);
        }
    }
}
