using DataAccessLayer.DbContext;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CustomAuthorizationFilter.Infrastructure
{
   public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                using (var context = new ApplicationDbContext())
                {
                    var userName = httpContext.User.Identity.Name;
                    var user = context.Users.FirstOrDefault(u => u.UserName == userName);

                    if (user != null)
                    {
                        var userRole = (from ur in context.UserRoles
                                        join r in context.Roles on ur.RoleId equals r.Id
                                        where ur.UserId == user.Id
                                        select r.Name).FirstOrDefault();

                        if (userRole != null)
                        {
                            foreach (var role in allowedroles)
                            {
                                if (role.Equals(userRole, StringComparison.OrdinalIgnoreCase))
                                {
                                    return true;
                                }
                            }
                        }
                    }

                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}
