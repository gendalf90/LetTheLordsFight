using UsersService.Controllers.Data;
using UsersService.Queries.GetCurrentToken;
using TokenQuery = UsersService.Queries.GetCurrentToken.Query;

namespace UsersService.Queries
{
    class Factory : IFactory
    {
        private readonly IGetCurrentUserStrategy currentUser;
        private readonly IGetTokenSigningKeyStrategy signingKey;

        public Factory(IGetCurrentUserStrategy currentUser, IGetTokenSigningKeyStrategy signingKey)
        {
            this.currentUser = currentUser;
            this.signingKey = signingKey;
        }

        public IQuery<TokenDto> CreateGetTokenQuery()
        {
            return new TokenQuery(currentUser, signingKey);
        }
    }
}
