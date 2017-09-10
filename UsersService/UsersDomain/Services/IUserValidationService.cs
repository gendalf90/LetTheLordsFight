using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Services
{
    public interface IUserValidationService
    {
        Task CurrentUserShouldBeSystemOrAdminAsync();
    }
}
