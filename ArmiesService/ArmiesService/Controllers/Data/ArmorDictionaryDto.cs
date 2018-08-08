namespace ArmiesService.Controllers.Data
{
    public class ArmorDictionaryDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public DefenceDictionaryDto[] Defence { get; set; }

        public string[] Tags { get; set; }
    }
}
