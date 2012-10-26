using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using System.Web.Security;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class IdentityController : RavenController
    {
        public ActionResult IsAuthenticated()
        {
            var isAuthenticated = this.Request.IsAuthenticated;
            return HttpDoc<bool>.OK(isAuthenticated).ToJson();
        }

        [HttpPost]
        public ActionResult Authenticate(string email, string password, bool rememberMe = false)
        {
            if (email.IsNullOrWhiteSpace())
                this.PropertyValidationMessage("Email", "Email is required");

            if (password.IsNullOrWhiteSpace())
                this.PropertyValidationMessage("Password", "Password is required");

            if (!ModelState.IsValid)
            {
                var errors = this.ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
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
                      usr.FirstName + " " + usr.LastName,  //user postSlug
                      DateTime.Now,
                      expiration,  // expiry
                      rememberMe,  //do not remember
                      Newtonsoft.Json.JsonConvert.SerializeObject(usr.GetUserInfo()),
                      "/");

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);

                    return HttpDoc<UserInfo>.OK(usr.GetUserInfo()).ToJson();
                }
                else if (status == AuthenticationResult.PasswordDoNotMatch)
                {
                    this.ValidationMessage(Local.Identity.Login.MsgPasswordDoNotMatch);
                }
                else if (status == AuthenticationResult.UsernameNotFound)
                {
                    this.ValidationMessage(Local.Identity.Login.MsgAccountNotExist);
                }
                else if (status == AuthenticationResult.AccountDisabled)
                {
                    this.ValidationMessage(Local.Identity.Login.MsgAccountDisabled);
                }
                else
                {
                    this.ValidationMessage(Local.Identity.Login.MsgUnknownError);
                }
            }
            else
            {
                this.ValidationMessage(res.Message);
            }

            //if this point is reach, there's an error happening in the validation
            var lastErrors = this.ProduceAJAXErrorMessage(ModelState);
            return HttpDoc<EmptyHttpReponse>.PreconditionFailed(lastErrors.ToJson()).ToJson();
        }
    }
}
