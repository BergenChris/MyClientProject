using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyClientProject.Models;
using System.Text.Json;


namespace MyClientProject.Filters
{
    public class KlantSessionAuthorizeAttribute : Attribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userJson = context.HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(userJson))
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
                return;
            }

            try
            {
                var user = JsonSerializer.Deserialize<User>(userJson);

                if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.UserEmail))
                {
                    // User exists but invalid (no name/email) → redirect
                    context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
                }

                // Else: valid user or guest with required info → allow
            }
            catch
            {
                // Session data is corrupt or wrong format → redirect
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
            }
        }
    }
}
