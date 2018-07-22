namespace ArmiesService.Queries.Army
{
    public class ArmyDto
    {
        public string OwnerLogin { get; set; }

        public SquadDto[] Squads { get; set; }
    }
}
