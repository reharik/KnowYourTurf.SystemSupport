using System.Collections.Generic;
using System.Web.Mvc;
using SystemSupport.Web.Menus;
using CC.Core.DomainTools;
using CC.Core.Services;
using Castle.Components.Validator;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.Html.Menu;

    using KnowYourTurf.Core;
    using KnowYourTurf.Core.Domain;

    public class OrthogonalController : KYTController
    {
        private readonly IMenuConfig _menuConfig;
        private readonly IRepository _repository;
        private readonly ISelectListItemService _selectListItemService;

        public OrthogonalController(IMenuConfig menuConfig,
            IRepository repository,
            ISelectListItemService selectListItemService)
        {
            _menuConfig = menuConfig;
            _repository = repository;
            _selectListItemService = selectListItemService;
        }

        public PartialViewResult SystemSupportHeader(ViewModel input)
        {
            var clientList = _selectListItemService.CreateList<Client>(x => x.Name, x => x.EntityId, true);
            HeaderViewModel model = new HeaderViewModel
            {
                User = (User)input.User,
                LoggedIn = User.Identity.IsAuthenticated,
                _ClientEntityIdList = clientList
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
        public IEnumerable<SelectListItem> _ClientEntityIdList { get; set; }
        public int ClientEntityId { get; set; }

    }
}