using UsersService.Controllers.Data;

namespace UsersService.Queries
{
    public interface IFactory
    {
        IQuery<TokenDto> CreateGetTokenQuery();
    }
}
