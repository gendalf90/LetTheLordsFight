using MongoDB.Bson;
using System.Threading.Tasks;
using System;

namespace StorageService.Events
{
    abstract class JsonReadHandler : IEventReader
    {
        protected IEventReader successor;

        public void SetSuccessor(IEventReader successor)
        {
            this.successor = successor;
        }

        public Event ReadFromJson(string json)
        {
            if(TryParse(json, out Event result))
            {
                return result;
            }
            else if(successor != null)
            {
                return successor.ReadFromJson(json);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        protected abstract bool TryParse(string json, out Event e);
    }
}
