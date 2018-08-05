namespace ArmiesService.Queries.Army
{
    public class ArmyQueryDto
    {
        public string OwnerLogin { get; set; }

        public SquadQueryDto[] Squads { get; set; }
    }
}
