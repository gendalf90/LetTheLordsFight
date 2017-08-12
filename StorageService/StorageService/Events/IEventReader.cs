using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    interface IEventReader
    {
        Event ReadFromJson(string json);
    }
}
