using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Middlewares
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeAttribute(string role = null)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Check if user is authenticated
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                // Not authenticated — return 401 Unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            // If role is specified, check if user has the role
            if (!string.IsNullOrEmpty(_role) && !user.IsInRole(_role))
            {
                // User does not have the required role — return 403 Forbidden
                context.Result = new ForbidResult();
            }
        }
    }
}
