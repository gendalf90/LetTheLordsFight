using System;
using System.Collections.Generic;
using System.Text;

namespace UsersDomain.Common
{
    public interface IUserPresentData
    {
        string Login { get; set; }

        string Type { get; set; }

        string StorageId { get; set; }

        string MapObjectId { get; set; }
    }
}
