using System;
using System.Web.Mvc;
using System.Web.Security;
using SystemSupport.Web.Services;

using Castle.Components.Validator;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core;

    using KnowYourTurf.Core;
    using KnowYourTurf.Core.Services;

    public class LoginController : Controller
    {
        private readonly ISecurityDataService _securityDataService;
        private readonly IAuthenticationContext _authenticationContext;

        public LoginController(ISecurityDataService securityDataService,
                               IAuthenticationContext authenticationContext)
        {
            _securityDataService = securityDataService;
            _authenticationContext = authenticationContext;
        }

        public ActionResult Login()
        {
            var loginViewModel = new LoginViewModel
                                     {
                                         Title = WebLocalizationKeys.LOGIN_SYSTEM_SUPPORT.ToString()
                                     };
            return View(loginViewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel input)
        {

            var notification = new Notification { Message = WebLocalizationKeys.INVALID_USERNAME_OR_PASSWORD.ToString() };
            try
            {
                if (input.HasCredentials())
                {
                    var redirectUrl = string.Empty;
                    var uli = _securityDataService.AuthenticateForUserId(input.UserName, input.Password,"");
                    if (uli != null)
                    {
                        if (uli.GetCurrentSubscription().ExpirationDate > DateTime.Now)
                        {
                            redirectUrl = _authenticationContext.ThisUserHasBeenAuthenticated(uli, input.RememberMe);
                            notification.Success = true;
                            notification.Message = string.Empty;
                            notification.Redirect = true;
                            notification.RedirectUrl = redirectUrl;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                notification = new Notification { Message = WebLocalizationKeys.ERROR_UNEXPECTED.ToString() };
                ex.Source = "CATCH RAISED";
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Json(notification);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();
            return RedirectToAction("Login");
        }
    }

    public class LoginViewModel : ViewModel
    {
        public string SiteName { get { return CoreLocalizationKeys.SITE_NAME.ToString(); } }
        [ValidateNonEmpty]
        public string UserName { get; set; }
        [ValidateNonEmpty]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string RegisterUrl { get; set; }
        public string ForgotPasswordUrl { get; set; }
        public string ForgotPasswordTitle { get; set; }
        
        public bool HasCredentials()
        {
            return UserName.IsNotEmpty() && Password.IsNotEmpty();
        }
    }

}