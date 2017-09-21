using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Common;
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
        public async Task CreateFromCommand_ValidCreateData_UserAddedToStore()
        {
            var addedToStoreUserRepositoryData = new Mock<IUserRepositoryData>().SetupAllProperties().Object;
            var store = new Mock<IUsersStore>();
            store.Setup(s => s.SaveAsync(It.IsAny<User>()))
                 .Callback<User>(r => r.FillRepositoryData(addedToStoreUserRepositoryData))
                 .Returns(Task.CompletedTask);
            var data = new CreateUserData { Login = TestLogin, Password = TestPassword };
            var command = new CreateUserCommand(store.Object, data);

            await command.ExecuteAsync();

            Assert.Equal(addedToStoreUserRepositoryData.Login, TestLogin);
            Assert.Equal(addedToStoreUserRepositoryData.Password, TestPassword);
            Assert.Empty(addedToStoreUserRepositoryData.Roles);
        }
    }
}
