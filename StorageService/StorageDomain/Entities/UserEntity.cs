namespace StorageDomain.Entities
{
    public class UserEntity
    {
        private string storageId;
        private string type;

        public UserEntity(string type, string storageId)
        {
            this.type = type;
            this.storageId = storageId;
        }

        public bool IsAdminOrSystem { get => type == "System" || type == "Admin"; }

        public bool IsOwnerOf(string storageId) => this.storageId == storageId;
    }
}
