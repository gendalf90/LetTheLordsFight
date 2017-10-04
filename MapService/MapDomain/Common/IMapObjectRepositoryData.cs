using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Common
{
    public interface IMapObjectRepositoryData
    {
        string Id { get; set; }

        float LocationX { get; set; }

        float LocationY { get; set; }

        float DestinationX { get; set; }

        float DestinationY { get; set; }
    }
}
