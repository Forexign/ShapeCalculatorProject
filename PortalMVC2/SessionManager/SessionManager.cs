using DataAccessLayer.DbContext;
using DataAccessLayer.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PortalMVC2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortalMVC2.SessionManager
{
    public class User
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> AllowedShapes { get; set; }
        public List<RoleActionPermission> AllowedActions { get; set; }
        public string UserRole { get; set; }
    }
    public static class SessionDataManager
    {

        private static User _user;
        public static User CurrentUser { get { return (User)System.Web.HttpContext.Current.Session["User"]; } }
        public static void PrepareSessionInformations(string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var dbuser = db.Users.Find(userId);
            var userRoles = db.UserRoles.Where(p => p.UserId == userId).FirstOrDefault();
            var role = db.Roles.Where(p => p.Id == userRoles.RoleId).Select(i => i.Id).FirstOrDefault();
            _user = new User()
            {
                UserID = userId,
                FirstName = dbuser.FirstName,
                LastName = dbuser.LastName,
                AllowedShapes = GetAllowedShapesForCurrentUser(db, role),
                AllowedActions = GetAuthorizedActions(db, role),
                UserRole = role ?? "",
            };
            System.Web.HttpContext.Current.Session["User"] = _user;
        }
        private static List<string> GetAllowedShapesForCurrentUser(ApplicationDbContext db, string userRoleId)
        {
            var roleIdOfUser = db.Roles.Where(i => i.Id == userRoleId).Select(i => i.Id);
            var allowedShapeIds = db.RoleShapePermissions
                                  .Where(r => roleIdOfUser.Contains(r.RoleId))
                                  .Select(r => r.ShapeType)
                                  .Cast<int>()
                                  .ToList();
            var allowedShapesNames = db.ShapeTypes.Where(i => allowedShapeIds.Contains(i.Id)).Select(i => i.Name).ToList();
            return allowedShapesNames;
        }
        private static List<RoleActionPermission> GetAuthorizedActions(ApplicationDbContext db, string UserRoleId)
        {
            var authorizedActions = db.RoleActionPermissions
                                    .Where(i => i.RoleId == UserRoleId)
                                    .ToList();
            return authorizedActions;
        }
    }
}