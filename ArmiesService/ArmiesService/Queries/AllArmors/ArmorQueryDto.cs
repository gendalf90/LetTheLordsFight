namespace ArmiesService.Queries.AllArmors
{
    public class ArmorQueryDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public DefenceQueryDto[] Defence { get; set; }

        public string[] Tags { get; set; }
    }
}
