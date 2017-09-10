using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Common;
using UsersDomain.Entities;
using UsersDomain.Exceptions;
using UsersDomain.Repositories;
using UsersDomain.Services;
using Xunit;

namespace Tests.Unit
{
    public class UserValidaitonServiceTest
    {
        [Fact]
        public async Task CheckSystemOrAdmin_UserTypeIsSimple_ThrowException()
        {
            var repositoryData = new Mock<IUserRepositoryData>()
                                            .SetupProperty(data => data.Type, "Simple")
                                            .Object;
            var user = new User(repositoryData);
            var repository = new Mock<IUsersRepository>();
            repository.Setup(rep => rep.GetCurrentAsync())
                      .Returns(Task.FromResult(user));
            var service = new UserValidationService(repository.Object);

            await Assert.ThrowsAsync<UserValidationException>(service.CurrentUserShouldBeSystemOrAdminAsync);
        }
    }
}
