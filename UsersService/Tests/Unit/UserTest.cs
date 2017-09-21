using Moq;
using UsersDomain.Common;
using UsersDomain.Entities;
using UsersDomain.Exceptions;
using UsersDomain.ValueTypes;
using Xunit;

namespace Tests.Unit
{
    public class UserTest
    {
        private const string TestLogin = "test@test.com";
        private const string TestPassword = "1q2w3e4r!";

        [Fact]
        public void FillRepositoryData_CreateFromRepositoryData_BaseAndResultDataAreEqual()
        {
            var baseRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupProperty(data => data.Login, "asdf@qwer.ru")
                                            .SetupProperty(data => data.Password, "1234_qwer")
                                            .SetupProperty(data => data.Roles, new [] { "Simple" })
                                            .Object;
            var resultRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupAllProperties()
                                            .Object;

            var user = new User(baseRepositoryData);
            user.FillRepositoryData(resultRepositoryData);

            Assert.Equal(baseRepositoryData.Login, resultRepositoryData.Login);
            Assert.Equal(baseRepositoryData.Password, resultRepositoryData.Password);
            Assert.Equal(baseRepositoryData.Roles, resultRepositoryData.Roles);
        }

        [Fact]
        public void Create_FromLoginAndPassword_RepositoryDataIsExpected()
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var resultRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupAllProperties()
                                            .Object;

            var user = new User(login, password);
            user.FillRepositoryData(resultRepositoryData);

            Assert.Equal(resultRepositoryData.Login, login.ToString());
            Assert.Equal(resultRepositoryData.Password, password.ToString());
            Assert.Empty(resultRepositoryData.Roles);
        }

        [Fact]
        public void ChangeRoles_RepositoryDataRolesAreExpected()
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var user = new User(login, password);
            var resultRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupAllProperties()
                                            .Object;

            user.AddRole(Role.Simple);
            user.AddRole(Role.Simple);
            user.AddRole(Role.System);
            user.RemoveRole(Role.Simple);
            user.FillRepositoryData(resultRepositoryData);

            Assert.Single(resultRepositoryData.Roles, "System");
        }

        [Fact]
        public void CheckCredentials_LoginAndPasswordValid_ResultIsTrue()
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var user = new User(login, password);

            var checkingResult = user.IsCredentialsValid(login, password);

            Assert.True(checkingResult);
        }
    }
}
