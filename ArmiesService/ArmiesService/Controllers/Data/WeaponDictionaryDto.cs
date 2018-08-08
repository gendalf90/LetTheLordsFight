namespace ArmiesService.Controllers.Data
{
    public class WeaponDictionaryDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public OffenceDictionaryDto[] Offence { get; set; }

        public string[] Tags { get; set; }
    }
}
