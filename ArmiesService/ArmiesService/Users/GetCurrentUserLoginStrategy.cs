using Microsoft.AspNetCore.Http;

namespace ArmiesService.Users
{
    class GetCurrentUserLoginStrategy : IGetCurrentUserLoginStrategy
    {
        private readonly IHttpContextAccessor contextAccessor;

        public GetCurrentUserLoginStrategy(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public string Get()
        {
            return contextAccessor.HttpContext
                                  .User
                                  .Identity
                                  .Name;
        }
    }
}
