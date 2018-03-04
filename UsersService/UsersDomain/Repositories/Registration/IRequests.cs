using System;
using System.Collections.Generic;
using System.Text;

namespace UsersDomain.Repositories.Registration
{
    public interface IRequests
    {
        void Save(RequestDto data); //unique index on login field
    }
}
