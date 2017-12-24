using LoginUser = UsersService.Queries.GetUserByLogin.User;
using CurrentUser = UsersService.Queries.GetCurrentUser.User;

namespace UsersService.Queries
{
    public interface IFactory
    {
        IQuery<LoginUser> CreateGetUserByLoginQuery(string login);

        IQuery<CurrentUser> CreateGetCurrentUserQuery();

        IQuery<string> CreateGetTokenQuery();
    }
}
