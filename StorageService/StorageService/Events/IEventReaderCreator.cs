using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    interface IEventReaderCreator
    {
        IEventReader Create();
    }
}
