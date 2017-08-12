using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Services
{
    interface IMapService
    {
        Task<MapObjectDto> GetByIdAsync(string id);
    }
}
