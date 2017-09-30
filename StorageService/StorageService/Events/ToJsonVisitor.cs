using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class ToJsonVisitor : IEventVisitor
    {
        private readonly ICollection<string> source;

        public ToJsonVisitor(ICollection<string> source)
        {
            this.source = source;
        }

        public async Task VisitAsync(SingleTransactionEvent e)
        {
            var result = CreateBase(e);
            result["Type"] = "SingleTransaction";
            result["TransactionType"] = e.TransactionType.ToString();
            result["ItemCount"] = e.ItemCount;
            result["ItemName"] = e.ItemName;
            await SaveResultAsync(result);
        }

        public async Task VisitAsync(CreateEvent e)
        {
            var result = CreateBase(e);
            result["Type"] = "Create";
            await SaveResultAsync(result);
        }

        public async Task VisitAsync(SnapshotEvent e)
        {
            var result = CreateBase(e);
            result["Type"] = "Snapshot";
            result["Items"] = JObject.FromObject(e.Items);
            await SaveResultAsync(result);
        }

        private JObject CreateBase(Event e)
        {
            return new JObject
            {
                ["_id"] = e.Id,
                ["StorageId"] = e.StorageId
            };
        }

        private async Task SaveResultAsync(JObject result)
        {
            source.Add(result.ToString());
            await Task.CompletedTask;
        }
    }
}
