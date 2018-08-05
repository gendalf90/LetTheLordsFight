namespace ArmiesDomain.Repositories.Weapons
{
    public class WeaponRepositoryDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public OffenceRepositoryDto[] Offence { get; set; }

        public string[] Tags { get; set; }
    }
}
