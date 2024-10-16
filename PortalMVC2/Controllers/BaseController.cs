using CustomAuthorizationFilter.Infrastructure;
using DataAccessLayer.DbContext;
using DataAccessLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PortalMVC2.SessionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PortalMVC2.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext db = new ApplicationDbContext();
        protected readonly UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;


            var AllowedActions = (Session.Count > 0) ? SessionDataManager.CurrentUser.AllowedActions : null;
            if (AllowedActions == null || !AllowedActions.Any())
            {
                filterContext.Result = View("UnAuthorized");
                return;
            }

            bool IsAuthenticated = AllowedActions.Any(i => i.ControllerName == controllerName && i.ActionName == actionName);
            if (!IsAuthenticated)
            {
                filterContext.Result = View("UnAuthorized");
                return;
            }

        }
        protected ActionResult UnAuthorized()
        {
            return View("UnAuthorized");
        }
    }
}
