using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PortalMVC2.Models;
using DataAccessLayer.Models;
using PortalMVC2.SessionManager;

public class AccountController : Controller
{
    private ApplicationSignInManager _signInManager;
    private ApplicationUserManager _userManager;

    public AccountController()
    {
    }

    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    public ApplicationSignInManager SignInManager
    {
        get
        {
            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        }
        private set
        {
            _signInManager = value;
        }
    }
    public ApplicationUserManager UserManager
    {
        get
        {
            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
        private set
        {
            _userManager = value;
        }
    }
    public ActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var userExists = await UserManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                ModelState.AddModelError("", "Bu E-Posta adresi zaten kullanımda.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //new users assigns to Normal role
                await UserManager.AddToRoleAsync(user.Id, "Normal");

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Hesabınızı Onaylayın", "Lütfen hesabınızı onaylamak için <a href=\"" + callbackUrl + "\">buraya tıklayınız</a>");

                return RedirectToAction("RegisterSuccess", "Account");
            }
            AddErrors(result);
        }


        return View(model);
    }
    public async Task<ActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error");
        }
        var result = await UserManager.ConfirmEmailAsync(userId, code);
        if (result.Succeeded)
        {
            //removes from the normal role and adds to member
            await UserManager.RemoveFromRoleAsync(userId, "Normal");
            await UserManager.AddToRoleAsync(userId, "Member");
        }
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    public ActionResult RegisterSuccess()
    {
        return View();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }
    }

    public ActionResult Login()
    {
        return View();
    }


    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginModel model, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await UserManager.FindByEmailAsync(model.Email);

        if (user != null && !await UserManager.IsEmailConfirmedAsync(user.Id))
        {
            ModelState.AddModelError("", "E-posta adresinizi doğrulamadan giriş yapamazsınız...");
            return View(model);
        }

        var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

        switch (result)
        {
            case SignInStatus.Success:
                string userId = SignInManager
                .AuthenticationManager
                .AuthenticationResponseGrant.Identity.GetUserId();

                SessionDataManager.PrepareSessionInformations(userId);
                return RedirectToAction("MyView", "Home");
            case SignInStatus.LockedOut:
                return View("Lockout");
            case SignInStatus.RequiresVerification:
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
            case SignInStatus.Failure:
            default:
                ModelState.AddModelError("", "E-Posta yada şifre hatalı!");
                return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Logout()
    {
        System.Web.HttpContext.Current.Session.Clear();
        SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        return RedirectToAction("Login", "Account");
    }
}
