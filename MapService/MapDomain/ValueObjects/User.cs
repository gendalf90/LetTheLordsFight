using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.ValueObjects
{
    public class User
    {
        private readonly string login;

        public User(string login, bool isSystem)
        {
            this.login = login;
            IsSystem = isSystem;
        }

        public bool IsSystem { get; private set; }

        public bool IsOwnerOf(string mapObjectId) => login == mapObjectId;
    }
}
