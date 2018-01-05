using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersService.Commands;
using UsersService.Common;
using UsersService.Users;
using Xunit;

namespace Tests.Unit
{
    public class CreateUserTest
    {
        private const string TestLogin = "test@test.com";
        private const string TestPassword = "1q2w3e4r!";

        [Fact]
        public async Task CreateFromCommand_ValidCreateData_UserHasBeenAddedToStore()
        {
            var addedToStoreUsers = new List<User>();
            var store = new Mock<IUsersStore>();
            store.Setup(s => s.AddAsync(It.IsAny<User>()))
                 .Callback<User>(addedToStoreUsers.Add)
                 .Returns(Task.CompletedTask);
            var data = new CreateUserData { Login = TestLogin, Password = TestPassword };
            var command = new CreateUserCommand(store.Object, data);

            await command.ExecuteAsync();

            Assert.Contains(addedToStoreUsers, user => user.Login == TestLogin);
        }
    }
}
