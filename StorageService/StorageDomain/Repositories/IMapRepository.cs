using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Repositories
{
    public interface IMapRepository
    {
        Task<Coordinate> GetStorageCoordinateAsync(string storageId);
    }
}
