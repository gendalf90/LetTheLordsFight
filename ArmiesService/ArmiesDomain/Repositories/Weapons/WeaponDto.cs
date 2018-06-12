namespace ArmiesDomain.Repositories.Weapons
{
    public class WeaponDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public OffenceDto[] Offence { get; set; }

        public string[] Tags { get; set; }
    }
}
