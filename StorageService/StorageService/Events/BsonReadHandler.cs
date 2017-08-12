using MongoDB.Bson;
using System.Threading.Tasks;
using System;

namespace StorageService.Events
{
    abstract class BsonReadHandler<T> : IEventReader where T: Event
    {
        protected IEventReader successor;

        public void SetSuccessor(IEventReader successor)
        {
            this.successor = successor;
        }

        public Event ReadFromJson(string json)
        {
            var document = BsonDocument.Parse(json);

            if (document.TryGetValue("Type", out var value) && value == nameof(T))
            {
                return ToEventFromBsonDocument(document);
            }
            else if (successor != null)
            {
                return successor.ReadFromJson(json);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        protected abstract T ToEventFromBsonDocument(BsonDocument document);
    }
}
