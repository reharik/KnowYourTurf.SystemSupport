using System.Collections.Generic;
using System.Web.Mvc;
using SystemSupport.Web.Menus;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.Html.Menu;

    using KnowYourTurf.Core;
    using KnowYourTurf.Core.Domain;

    public class OrthogonalController : KYTController
    {
        private readonly IMenuConfig _menuConfig;

        public OrthogonalController(IMenuConfig menuConfig)
        {
            _menuConfig = menuConfig;
        }

        public PartialViewResult SystemSupportHeader(ViewModel input)
        {
            HeaderViewModel model = new HeaderViewModel
            {
                User = (User)input.User,
                LoggedIn = User.Identity.IsAuthenticated,
            };
            return PartialView(model);
        }

        //remove true param when permissions are implemented
        public ActionResult SystemSupportMenu()
        {
            return PartialView(new MenuViewModel
            {
                MenuItems = _menuConfig.Build()
            });
        }
    }

    public class MenuViewModel
    {
        public IList<MenuItem> MenuItems { get; set; }
    }

    public class HeaderViewModel : PartialViewResult
    {
        public string SiteName { get { return CoreLocalizationKeys.SITE_NAME.ToString(); } }
        public User User { get; set; }
        public bool LoggedIn { get; set; }
        public string UserProfileUrl { get; set; }
    }
}