using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyClientProject.Models;
using System.Text.Json;


namespace MyClientProject.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class KlantSessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly bool _requireEmployer;

        // By default, user is required but not employer role
        public KlantSessionAuthorizeAttribute() => _requireEmployer = false;

        // If true, access only allowed for Employers
        public KlantSessionAuthorizeAttribute(bool requireEmployer) => _requireEmployer = requireEmployer;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userJson = context.HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(userJson))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            try
            {
                var user = JsonSerializer.Deserialize<User>(userJson);

                if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.UserEmail))
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
                }

                if (_requireEmployer && !string.Equals(user.Role, "Employer", StringComparison.OrdinalIgnoreCase))
                {
                    // User is logged in but not Employer
                    context.Result = new ForbidResult(); // or Redirect if you prefer
                }

                // else: user is valid, and if required, has Employer role
                // allow access
            }
            catch
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
