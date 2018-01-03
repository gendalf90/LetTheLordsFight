using System;
using System.Collections.Generic;
using System.Text;
using UsersDomain.Exceptions;
using UsersDomain.ValueTypes;
using Xunit;

namespace Tests.Unit
{
    public class LoginTest
    {
        [Theory]
        [InlineData("yandex@yandex.ru")]
        [InlineData("google@gmail.com")]
        [InlineData("foo-bar.baz@example.com")]
        [InlineData("asdfasdfasdfassdfasdfsdfadfasdfasdfasdfasdfasdfasdfasddasdfasdfasdfasdfddddasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfafdadfasdfasdfafasdfasdfasdfas@aasdfadfasdfasdfaasdfasdfasdfaasdfsddsfsdfs.ru")]
        public void Create_IsValid_ShouldCreate(string login)
        {
            var created = new Login(login);

            Assert.Equal(created.ToString(), login);
        }

        [Theory]
        [InlineData("asdf")]
        [InlineData("qwer.com")]
        [InlineData("qwer@com")]
        [InlineData("asdf:qwer@mail.ru")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("asdfasdfasdfassdfasdfsdfadfasdfasdfasdfasdfasdfasdfasddasdfasdfasdfasdfddddasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfafdadfasdfasdfafasdfasdfasdfas@aasdfadfasdfasdfaasdfasdfasdfaasdfsddsfsdfsa.ru")]
        public void Create_IsInvalid_ShouldThrowException(string login)
        {
            Assert.Throws<LoginException>(() => new Login(login));
        }

        [Theory]
        [InlineData("asdf@asdf.ru", "asdf@asdf.ru", true)]
        [InlineData("qWEr@asdf.RU", "qwer@ASDF.ru", true)]
        [InlineData("zxcv@zxcv.com", "zxcv@zxcv.ru", false)]
        public void Equal_ComparisonResultIsExpected(string loginOne, string loginTwo, bool expectedResult)
        {
            var testLoginOne = new Login(loginOne);
            var testLoginTwo = new Login(loginTwo);

            var comparisonResult = testLoginOne.Equals(testLoginTwo);

            Assert.Equal(comparisonResult, expectedResult);
        }
    }
}
