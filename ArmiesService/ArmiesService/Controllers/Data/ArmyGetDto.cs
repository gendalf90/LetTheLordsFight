namespace ArmiesService.Controllers.Data
{
    public class ArmyGetDto
    {
        public string OwnerLogin { get; set; }

        public SquadGetDto[] Squads { get; set; }
    }
}
