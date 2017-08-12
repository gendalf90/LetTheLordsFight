using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageDomain.Entities;

namespace StorageService.Events
{
    class EventVisitorFactory : IEventVisitorFactory
    {
        public IEventVisitor CreateApplyVisitor(ICollection<StorageEntity> source)
        {
            return new ApplyVisitor(source);
        }

        public IEventVisitor CreateToJsonVisitor(ICollection<string> source)
        {
            return new ToJsonVisitor(source);
        }
    }
}
