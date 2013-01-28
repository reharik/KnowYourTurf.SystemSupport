using System.Collections.Generic;
using SystemSupport.Web.Areas.Permissions.Controllers;
using SystemSupport.Web.Controllers;

namespace SystemSupport.Web.Menus
{
    using CC.Core.Html.Menu;

    public class SystemSupportMenu : IMenuConfig
    {
        private readonly IMenuBuilder _builder;

        public SystemSupportMenu(IMenuBuilder builder)
        {
            _builder = builder;
        }

        public IList<MenuItem> Build(bool withoutPermissions = false)
        {
            return DefaultMenubuilder(withoutPermissions);
        }

        private IList<MenuItem> DefaultMenubuilder(bool withoutPermissions = false)
        {
            return _builder

                .CreateTagNode<UserListController>(WebLocalizationKeys.USERS)
                .CreateTagNode<CompanyListController>(WebLocalizationKeys.Clients)
                .CreateTagNode<PromotionListController>(WebLocalizationKeys.PROMOTION)
                .CreateTagNode<PendingChargesListController>(WebLocalizationKeys.PENDING_CHARGES)
                .CreateTagNode<LoginStatisticsListController>(WebLocalizationKeys.LOGIN_INFORMATION)
                .CreateTagNode<UserGroupListController>(WebLocalizationKeys.PERMISSION_USER_GROUPS)
                .CreateTagNode<SiteConfigurationController>(WebLocalizationKeys.SYSTEM_OFFLINE)
                .MenuTree(withoutPermissions);
        }
    }
}