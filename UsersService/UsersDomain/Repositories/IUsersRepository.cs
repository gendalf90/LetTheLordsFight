using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Entities;

namespace UsersDomain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetByLoginAsync(string login);

        Task<User> GetByStorageIdAsync(string storageId);

        Task<User> GetByMapObjectIdAsync(string mapObjectId);

        Task SaveAsync(User user);

        Task<User> GetCurrentAsync();
    }
}
