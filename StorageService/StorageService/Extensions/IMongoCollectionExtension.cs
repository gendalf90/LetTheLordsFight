using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Extensions
{
    public static class IMongoCollectionExtension
    {
        public static IFindFluent<T, T> GetAll<T>(this IMongoCollection<T> collection)
        {
            var filter = Builders<T>.Filter.Empty;
            return collection.Find(filter);
        }

        public static IFindFluent<T, T> FindById<T, TKey>(this IMongoCollection<T> collection, TKey id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return collection.Find(filter);
        }
    }
}
