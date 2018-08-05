namespace ArmiesDomain.Repositories.Armors
{
    public class ArmorRepositoryDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public DefenceRepositoryDto[] Defence { get; set; }

        public string[] Tags { get; set; }
    }
}
