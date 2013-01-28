using System;
using System.Web;
using System.Web.Security;

namespace SystemSupport.Web.Services
{
    using KnowYourTurf.Core.Domain;

    public interface IAuthenticationContext
    {
        string ThisUserHasBeenAuthenticated(UserLoginInfo loginInfo, bool rememberMe);
        void SignOut();
    }

    public class WebAuthenticationContext : IAuthenticationContext
    {
        public string ThisUserHasBeenAuthenticated(UserLoginInfo loginInfo, bool rememberMe)
        {
            string userData = String.Empty;
            userData = userData + "UserId=" + loginInfo.User.EntityId + "|ClientId=" + loginInfo.ClientId + "|LoginInfoId=" + loginInfo.EntityId;
            var ticket = new FormsAuthenticationTicket(1, loginInfo.User.FullNameLNF, DateTime.Now, DateTime.Now.AddMinutes(30),
                                                       rememberMe, userData);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
            return FormsAuthentication.GetRedirectUrl(loginInfo.User.FullNameLNF, false);
        }

        public void SignOut()
        {
            SignOutFunc();
        }

        public Action<string, bool> SetAuthCookieFunc = FormsAuthentication.SetAuthCookie;
        public Action SignOutFunc = FormsAuthentication.SignOut;
    }
}