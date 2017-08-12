using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Services
{
    public interface IUserValidationService
    {
        void CurrentUserShouldBeSystemOrAdmin();

        void CurrentUserShouldBeOwnerOfThisMapObject(string mapObjectId);
    }
}
