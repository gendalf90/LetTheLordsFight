using StorageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    interface IEventVisitorFactory
    {
        IEventVisitor CreateApplyVisitor(ICollection<Storage> source);

        IEventVisitor CreateToJsonVisitor(ICollection<string> source);

        //и т.д. в том числе перенести из storages
    }
}
