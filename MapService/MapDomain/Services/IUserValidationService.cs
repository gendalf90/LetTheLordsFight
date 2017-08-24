using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Services
{
    public interface IUserValidationService
    {
        void CurrentCanCreateMapObject();

        void CurrentCanChangeDestinationForThisMapObject(string mapObjectId);

        void CurrentCanViewThisMapObject(string mapObjectId);

        void CurrentCanUpdateMap();
    }
}
