using Sif.Framework.Service.Authentication;
using Sif.Framework.Utils;
using Sif.Framework.WebApi.ActionResults;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Sif.Framework.WebApi.Filters
{

    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {

        //[PropertyInject]
        //private IAuthenticationService authService;

        public bool AllowMultiple
        {

            get
            {
                return false;
            }

        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            IAuthenticationService authService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAuthenticationService)) as IAuthenticationService;

            if (!authService.VerifyAuthenticationHeader(context.Request.Headers.Authorization))
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", context.Request);
            }

            return;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

    }

}
