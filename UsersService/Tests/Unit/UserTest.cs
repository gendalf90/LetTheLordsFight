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
        public const string TestLogin = "test@test.com";
        public const string TestPassword = "1q2w3e4r!";

        [Fact]
        public void FillRepositoryData_CreateFromRepositoryData_BaseAndResultDataAreEqual()
        {
            var baseRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupProperty(data => data.Login, "asdf@qwer.ru")
                                            .SetupProperty(data => data.Password, "1234_qwer")
                                            .SetupProperty(data => data.Type, "Simple")
                                            .SetupProperty(data => data.MapObjectId, "1234")
                                            .SetupProperty(data => data.StorageId, "5678")
                                            .Object;
            var resultRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupAllProperties()
                                            .Object;

            var user = new User(baseRepositoryData);
            user.FillRepositoryData(resultRepositoryData);

            Assert.Equal(baseRepositoryData.Login, resultRepositoryData.Login);
            Assert.Equal(baseRepositoryData.Password, resultRepositoryData.Password);
            Assert.Equal(baseRepositoryData.Type, resultRepositoryData.Type);
            Assert.Equal(baseRepositoryData.MapObjectId, resultRepositoryData.MapObjectId);
            Assert.Equal(baseRepositoryData.StorageId, resultRepositoryData.StorageId);
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
            Assert.Null(resultRepositoryData.Type);
            Assert.Null(resultRepositoryData.MapObjectId);
            Assert.Null(resultRepositoryData.StorageId);
        }

        [Fact]
        public void Create_FromLoginAndPassword_PresentDataIsExpected()
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var resultPresentData = new Mock<IUserPresentData>()
                                            .SetupAllProperties()
                                            .Object;

            var user = new User(login, password);
            user.FillPresentData(resultPresentData);

            Assert.Equal(resultPresentData.Login, login.ToString());
            Assert.Null(resultPresentData.Type);
            Assert.Null(resultPresentData.MapObjectId);
            Assert.Null(resultPresentData.StorageId);
        }

        [Fact]
        public void Change_Something_ChangedFieldsAreInTheRepositoryData()
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var user = new User(login, password);
            var resultRepositoryData = new Mock<IUserRepositoryData>()
                                            .SetupAllProperties()
                                            .Object;

            user.ChangeType(UserType.Admin);
            user.ChangeStorage("1234");
            user.ChangeMapObject("qwer");
            user.FillRepositoryData(resultRepositoryData);

            Assert.Equal(resultRepositoryData.Type, "Admin");
            Assert.Equal(resultRepositoryData.MapObjectId, "qwer");
            Assert.Equal(resultRepositoryData.StorageId, "1234");
        }

        [Fact]
        public void Change_Something_ChangedFieldsAreInThePresentData()
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var user = new User(login, password);
            var resultPresentData = new Mock<IUserPresentData>()
                                            .SetupAllProperties()
                                            .Object;

            user.ChangeType(UserType.Admin);
            user.ChangeStorage("1234");
            user.ChangeMapObject("qwer");
            user.FillPresentData(resultPresentData);

            Assert.Equal(resultPresentData.Type, "Admin");
            Assert.Equal(resultPresentData.MapObjectId, "qwer");
            Assert.Equal(resultPresentData.StorageId, "1234");
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

        [Theory]
        [InlineData(UserType.Admin, true)]
        [InlineData(UserType.System, true)]
        [InlineData(UserType.Simple, false)]
        public void ChangeType_CheckSystemOrAdmin_ReturnExpectedResult(UserType type, bool expected)
        {
            var login = new Login(TestLogin);
            var password = new Password(TestPassword);
            var user = new User(login, password);

            user.ChangeType(type);
            var checkResult = user.IsSystemOrAdmim;

            Assert.Equal(checkResult, expected);
        }
    }
}
