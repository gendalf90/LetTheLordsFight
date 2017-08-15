using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.ValueObjects
{
    public class User
    {
        private readonly string mapObjectId;
        private readonly string type;

        public User(string type, string mapObjectId)
        {
            this.type = type;
            this.mapObjectId = mapObjectId;
        }

        public bool IsAdminOrSystem { get => type == "System" || type == "Admin"; }

        public bool IsOwnerOf(string mapObjectId) => this.mapObjectId == mapObjectId;
    }
}
