using ArmiesDomain.Repositories.Armors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Armor
    {
        public string Name { get; set; }

        public static Task<Armor> LoadAsync(IArmors repository, string name)
        {
            throw new NotImplementedException();
        }
    }
}
