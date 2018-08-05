namespace ArmiesService.Queries.Army
{
    public class SquadQueryDto
    {
        public string Type { get; set; }

        public int Quantity { get; set; }

        public string[] Weapons { get; set; }

        public string[] Armors { get; set; }
    }
}
