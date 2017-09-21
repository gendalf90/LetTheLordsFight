using System;
using System.Collections.Generic;
using System.Linq;
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
        private HashSet<Role> roles;

        public User(Login login, Password password)
        {
            this.login = login.ToString();
            this.password = password.ToString();
            roles = new HashSet<Role>();
        }

        public User(IUserRepositoryData data)
        {
            login = data.Login;
            password = data.Password;
            roles = data.Roles.Select(Enum.Parse<Role>).ToHashSet();
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

        public void AddRole(Role role)
        {
            roles.Add(role);
        }

        public void RemoveRole(Role role)
        {
            roles.Remove(role);
        }

        public void FillRepositoryData(IUserRepositoryData repositoryData)
        {
            repositoryData.Login = login;
            repositoryData.Password = password;
            repositoryData.Roles = roles.Select(role => role.ToString());
        }
    }
}
