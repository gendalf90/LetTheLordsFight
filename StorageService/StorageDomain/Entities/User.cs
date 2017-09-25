namespace StorageDomain.Entities
{
    public class User
    {
        private readonly string login;

        public User(string login, bool isSystem)
        {
            this.login = login;
            IsSystem = isSystem;
        }

        public bool IsOwnerOf(string storageId)
        {
            return login == storageId;
        }

        public bool IsSystem { get; private set; }
    }
}
