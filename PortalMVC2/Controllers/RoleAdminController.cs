using DevExpress.Web.Mvc;
using CustomAuthorizationFilter.Infrastructure;
using DataAccessLayer.DbContext;
using DataAccessLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PortalMVC2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using DevExpress.XtraRichEdit.Import.Rtf;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PortalMVC2.Controllers
{
    public class RoleAdminController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private const string KareId = "1"; // db.ShapeTypes.Where(i => i.Name == 'Kare').Select(i => i.Id).FirstOrDefault(); ??
        private const string DikdortgenId = "2";
        private const string DaireId = "3";
        private const string SilindirId = "4";
        private const string KupId = "5";
        public RoleAdminController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }

        [HttpGet]
        public ActionResult AssignRole()
        {
            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();

            var model = users.Select(u => new UserRoleAssignmentViewModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                CurrentRole = userManager.GetRoles(u.Id).FirstOrDefault(),
                Roles = new SelectList(roles, "Name", "Name")
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRole(List<UserRoleAssignmentViewModel> model)
        {
            foreach (var item in model)
            {
                var roles = userManager.GetRoles(item.UserId).ToArray();

                if (roles.Length > 0)
                {
                    userManager.RemoveFromRoles(item.UserId, roles);
                }

                userManager.AddToRole(item.UserId, item.CurrentRole);
            }

            return RedirectToAction("AssignRole");
        }
        public ActionResult ManageRoleShapePermissions()
        {
            return View();
        }
        // Role şekil atama ile ilgili gridview!
        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var roles = db.Roles.ToList();
            var permissions = db.RoleShapePermissions.ToList();

            var model = roles.Select(role => new RoleShapeViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                CanCalculateKare = permissions.Any(p => p.RoleId == role.Id && p.ShapeType == KareId),
                CanCalculateDikdortgen = permissions.Any(p => p.RoleId == role.Id && p.ShapeType == DikdortgenId),
                CanCalculateDaire = permissions.Any(p => p.RoleId == role.Id && p.ShapeType == DaireId),
                CanCalculateSilindir = permissions.Any(p => p.RoleId == role.Id && p.ShapeType == SilindirId),
                CanCalculateKup = permissions.Any(p => p.RoleId == role.Id && p.ShapeType == KupId)
            }).ToList();

            return PartialView("~/Views/RoleAdmin/_GridViewPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] PortalMVC2.Models.RoleShapeViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = roleManager.Create(new IdentityRole(item.RoleName));

                    if (result.Succeeded)
                    {
                        var createdRole = roleManager.FindByName(item.RoleName);

                        SetDefaultActions(createdRole.Id);
                        AddShapePermission(createdRole.Id, item.CanCalculateKare, KareId);
                        AddShapePermission(createdRole.Id, item.CanCalculateDikdortgen, DikdortgenId);
                        AddShapePermission(createdRole.Id, item.CanCalculateDaire, DaireId);
                        AddShapePermission(createdRole.Id, item.CanCalculateSilindir, SilindirId);
                        AddShapePermission(createdRole.Id, item.CanCalculateKup, KupId);

                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        foreach (var i in result.Errors)
                        {
                            ModelState.AddModelError("", i);
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return GridViewPartial();
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] PortalMVC2.Models.RoleShapeViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await UpdatePermission(item.RoleId, KareId, item.CanCalculateKare);
                    await UpdatePermission(item.RoleId, DikdortgenId, item.CanCalculateDikdortgen);
                    await UpdatePermission(item.RoleId, DaireId, item.CanCalculateDaire);
                    await UpdatePermission(item.RoleId, SilindirId, item.CanCalculateSilindir);
                    await UpdatePermission(item.RoleId, KupId, item.CanCalculateKup);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return GridViewPartial();
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> GridViewPartialDelete(string roleId)
        {
            roleId = roleId.Replace("\"", "");
            if (roleId != null)
            {
                try
                {
                    var role = db.Roles.Where(i => i.Id == roleId).FirstOrDefault();
                    if (role != null && role.Name != "Admin")
                    {
                        //Silinen role tanımlanan Action yetkileri siliniyor.
                        var RelatedActions = db.RoleActionPermissions.Where(i => i.RoleId == role.Id).ToList();
                        db.RoleActionPermissions.RemoveRange(RelatedActions);

                        //Silinen rolün Şekil yetkileri siliniyor.
                        var RelatedShapePerms = db.RoleShapePermissions.Where(i => i.RoleId == role.Id).ToList();
                        db.RoleShapePermissions.RemoveRange(RelatedShapePerms);

                        //Silinen role sahip kullanıcılar Member Rolüne atanıyor.
                        var UsersHaveTheRole = db.UserRoles.Where(i => i.RoleId == roleId).ToList();
                        if (UsersHaveTheRole.Any())
                        {
                            foreach (var user in UsersHaveTheRole)
                            {
                                userManager.RemoveFromRoles(user.UserId);
                                userManager.AddToRole(user.UserId, "Member");
                            }
                        }

                        db.Roles.Remove(role);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return GridViewPartial();
        }
        //Kullanıcılara yetki atama ile ilgili gridView!
        [ValidateInput(false)]
        public ActionResult GridView2Partial()
        {
            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();
            ViewBag.AllRoles = roles.Select(i => i.Name).ToList();
            var model = users.Select(u => new UserRoleAssignmentViewModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                CurrentRole = userManager.GetRoles(u.Id).FirstOrDefault(),
                Roles = new SelectList(roles, "Name", "Name")
            }).ToList();

            return PartialView("~/Views/RoleAdmin/_GridView2Partial.cshtml", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridView2PartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] PortalMVC2.Models.UserRoleAssignmentViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var roles = userManager.GetRoles(item.UserId).ToArray();

                    if (roles.Length > 0)
                    {
                        userManager.RemoveFromRoles(item.UserId, roles);
                    }

                    userManager.AddToRole(item.UserId, item.CurrentRole);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return GridView2Partial();
        }
        private async Task UpdatePermission(string roleId, string shapeType, bool canCalculate)
        {
            var permission = db.RoleShapePermissions
                              .FirstOrDefault(p => p.RoleId == roleId && p.ShapeType == shapeType);

            if (canCalculate && permission == null)
            {
                db.RoleShapePermissions.Add(new RoleShapePermission
                {
                    RoleId = roleId,
                    ShapeType = shapeType
                });
            }
            else if (!canCalculate && permission != null)
            {
                db.RoleShapePermissions.Remove(permission);
            }

            await db.SaveChangesAsync();
        }
        private List<RoleActionPermission> SetDefaultActions(string roleId)
        {
            var defaultPermissions = new List<RoleActionPermission>
            {
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "MyView"
                },
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "Success"
                },
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "TotalGridViewPartial"
                },
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "TotalGridViewPartialAddNew"
                },
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "TotalGridViewPartialUpdate"
                },
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "TotalGridViewPartialDelete"
                },
                new RoleActionPermission
                {
                    RoleId = roleId,
                    ControllerName = "Home",
                    ActionName = "SendAction"
                },
            };
            db.RoleActionPermissions.AddRange(defaultPermissions);
            return defaultPermissions;
        }
        private void AddShapePermission(string roleId, bool canCalculate, string shapeType)
        {
            if (canCalculate)
            {
                db.RoleShapePermissions.Add(new RoleShapePermission
                {
                    RoleId = roleId,
                    ShapeType = shapeType
                });
            }
        }
    }
}