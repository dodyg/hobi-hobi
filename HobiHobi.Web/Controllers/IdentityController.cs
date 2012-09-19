using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Core.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HobiHobi.Web.Controllers
{
    public class IdentityController : RavenController
    {
        public ActionResult Index()
        {
            return Content(HobiHobi.Core.Identity.User.HashPassword("hello world"));   
        }

        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            //make sure the email has not been used before
            var usr = this.RavenSession.Query<User>().Where(x => x.Email == user.Email).FirstOrDefault();

            if (usr != null) 
                this.PropertyValidationMessage("Email", HobiHobi.Core.Validators.Resources.UserViewModelValidator.RepeatEmail);

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var u = user.GetUserForCreation();

            RavenSession.Store(u);
            SaveChangesAndTerminate();
            this.FlashSuccess(Global.Messages.RegistrationSuccessful);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Authenticate(string email, string password, bool rememberMe = false)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index", "Home");
            }
        
            var auth = new Authentication(this.RavenSession);
            var res = auth.Authenticate(email, password);

            if (res.IsTrue)
            {
                var status = res.Status;

                if (status == AuthenticationResult.OK)
                {
                    var usr = res.Value;
                    var roles = usr.Level.ToString();
                    var expiration = rememberMe ? DateTime.Now.AddDays(10) : DateTime.Now.AddMinutes(30);

                    var authTicket = new FormsAuthenticationTicket(
                      1,
                      usr.FirstName + " " + usr.LastName,  //user id
                      DateTime.Now,
                      expiration,  // expiry
                      rememberMe,  //do not remember
                      Newtonsoft.Json.JsonConvert.SerializeObject(usr.GetUserInfo()),
                      "/");

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);

                    return Redirect("/Manage");
                }
                else if (status == AuthenticationResult.PasswordDoNotMatch)
                {
                    this.FlashError(Local.Identity.Login.MsgPasswordDoNotMatch);
                    return RedirectToAction("Index", "Home");
                }
                else if (status == AuthenticationResult.UsernameNotFound)
                {
                    this.FlashError(Local.Identity.Login.MsgAccountNotExist);
                    return RedirectToAction("Index", "Home");
                }
                else if (status == AuthenticationResult.AccountDisabled)
                {
                    this.FlashError(Local.Identity.Login.MsgAccountDisabled);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.FlashError(Local.Identity.Login.MsgUnknownError);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                this.FlashError(res.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            this.FlashSuccess(Local.Identity.Logout.MsgLogout);
            return RedirectToAction("Index", "Home");
        }
    }
}
