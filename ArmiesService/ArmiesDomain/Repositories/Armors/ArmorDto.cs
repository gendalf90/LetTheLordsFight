namespace ArmiesDomain.Repositories.Armors
{
    public class ArmorDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public DefenceDto[] Defence { get; set; }

        public string[] Tags { get; set; }
    }
}
