using System.Collections.Generic;
using SystemSupport.Web.Areas.Permissions.Controllers;
using SystemSupport.Web.Controllers;

namespace SystemSupport.Web.Menus
{
    using CC.Core.Html.Menu;
    using CC.Security;

    using KnowYourTurf.Core.Services;

    public class SystemSupportMenu : IMenuConfig
    {
        private readonly IMenuBuilder _builder;

        private readonly ISessionContext _sessionContext;

        public SystemSupportMenu(IMenuBuilder builder, ISessionContext sessionContext)
        {
            _builder = builder;
            _sessionContext = sessionContext;
        }

        public IList<MenuItem> Build(bool withoutPermissions = false)
        {
            IUser user = null;
            if (!withoutPermissions)
            {
                user = _sessionContext.GetCurrentUser();
            }
            return DefaultMenubuilder(user);
        }

        private IList<MenuItem> DefaultMenubuilder(IUser user = null)
        {
            return _builder
                .CreateTagNode<ClientListController>(WebLocalizationKeys.HOME)
                .CreateTagNode<UserListController>(WebLocalizationKeys.USERS)
                .CreateTagNode<LoginStatisticsListController>(WebLocalizationKeys.LOGIN_INFORMATION)
                .CreateTagNode<UserGroupListController>(WebLocalizationKeys.PERMISSION_USER_GROUPS)
                .CreateTagNode<SiteConfigurationController>(WebLocalizationKeys.SYSTEM_OFFLINE)
                .MenuTree(user);
        }
    }
}