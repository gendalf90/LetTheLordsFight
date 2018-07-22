using Microsoft.AspNetCore.Http;

namespace ArmiesService.Common
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
