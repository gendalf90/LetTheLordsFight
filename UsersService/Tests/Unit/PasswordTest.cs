using System;
using System.Collections.Generic;
using System.Text;
using UsersDomain.Exceptions;
using UsersDomain.ValueTypes;
using Xunit;

namespace Tests.Unit
{
    public class PasswordTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1+a")]
        [InlineData("ass!76y")]
        public void Create_LessThanEightSymbolsOrEmpty_ThrowException(string value)
        {
            Assert.Throws<PasswordInvalidException>(() => new Password(value));
        }

        [Theory]
        [InlineData("1234567a!0123456")]
        [InlineData("asdf1234asdf78+=")]
        public void Create_MoreThanFifteenSymbols_ThrowException(string value)
        {
            Assert.Throws<PasswordInvalidException>(() => new Password(value));
        }

        [Theory]
        [InlineData("asdf_asdf")]
        [InlineData("!_urul,_l")]
        public void Create_NoDigits_ThrowException(string value)
        {
            Assert.Throws<PasswordInvalidException>(() => new Password(value));
        }

        [Theory]
        [InlineData("!_!+=^&*$")]
        [InlineData("5465^5432")]
        public void Create_NoLetters_ThrowException(string value)
        {
            Assert.Throws<PasswordInvalidException>(() => new Password(value));
        }

        [Theory]
        [InlineData("asdf1234")]
        [InlineData("90as90df")]
        public void Create_NoSpecialSymbols_ThrowException(string value)
        {
            Assert.Throws<PasswordInvalidException>(() => new Password(value));
        }

        [Fact]
        public void Create_HasColon_ThrowException()
        {
            var withColonPassword = "(90asdf:90qwer)";

            Assert.Throws<PasswordInvalidException>(() => new Password(withColonPassword));
        }

        [Theory]
        [InlineData("asd__123")]
        [InlineData("000as+df000")]
        [InlineData("<password&1234>")]
        public void Create_Valid_ShouldCreate(string value)
        {
            var created = new Password(value);

            Assert.Equal(created.ToString(), value);
        }

        [Theory]
        [InlineData("asdf_asd1", "asdf_asd1", true)]
        [InlineData("Qwer.1234", "qwer.1234", false)]
        public void Equal_ComparisonResultIsExpected(string passwordOne, string passwordTwo, bool expectedResult)
        {
            var testPasswordOne = new Password(passwordOne);
            var testPasswordTwo = new Password(passwordTwo);

            var comparisonResult = testPasswordOne.Equals(testPasswordTwo);

            Assert.Equal(comparisonResult, expectedResult);
        }
    }
}
