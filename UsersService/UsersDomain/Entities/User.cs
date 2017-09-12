using System;
using System.Collections.Generic;
using System.Text;
using UsersDomain.Common;
using UsersDomain.Exceptions;
using UsersDomain.ValueTypes;

namespace UsersDomain.Entities
{
    public class User
    {
        private string login;
        private string password;
        private string type;
        private string storageId;
        private string mapObjectId;

        public User(Login login, Password password)
        {
            this.login = login.ToString();
            this.password = password.ToString();
        }

        public User(IUserRepositoryData data)
        {
            login = data.Login;
            password = data.Password;
            type = data.Type;
            storageId = data.StorageId;
            mapObjectId = data.MapObjectId;
        }

        public string Login
        {
            get
            {
                return login.ToString();
            }
        }

        public bool IsCredentialsValid(Login login, Password password)
        {
            return this.login == login.ToString() && this.password == password.ToString();
        }

        public bool IsSystemOrAdmim
        {
            get => type == UserType.Admin.ToString() || type == UserType.System.ToString();
        }

        public void ChangeType(UserType type)
        {
            this.type = type.ToString();
        }

        public void ChangeStorage(string id)
        {
            storageId = id;
        }

        public void ChangeMapObject(string id)
        {
            mapObjectId = id;
        }

        public void FillPresentData(IUserPresentData presentData)
        {
            presentData.Login = login;
            presentData.Type = type;
            presentData.MapObjectId = mapObjectId;
            presentData.StorageId = storageId;
        }

        public void FillRepositoryData(IUserRepositoryData repositoryData)
        {
            repositoryData.Login = login;
            repositoryData.Password = password;
            repositoryData.Type = type;
            repositoryData.StorageId = storageId;
            repositoryData.MapObjectId = mapObjectId;
        }
    }
}
