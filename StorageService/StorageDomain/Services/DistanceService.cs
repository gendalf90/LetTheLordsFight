using StorageDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public class DistanceService : IDistanceService
    {
        private readonly IMapRepository map;

        public DistanceService(IMapRepository map)
        {
            this.map = map;
        }

        public async Task<bool> IsDistanceBeetwenStoragesSufficientForTransactionAsync(string fromId, string toId)
        {
            var coordinateFrom = await map.GetCoordinateAsync(fromId);
            var coordinateTo = await map.GetCoordinateAsync(toId);
            return coordinateFrom.Equals(coordinateTo);
        }
    }
}
