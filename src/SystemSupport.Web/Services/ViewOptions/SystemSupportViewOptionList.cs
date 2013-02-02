using System.Collections.Generic;

using SystemSupport.Web.Areas.Permissions.Controllers;
using SystemSupport.Web.Areas.Permissions.Grids;
using SystemSupport.Web.Controllers;

namespace SystemSupport.Web.Services.ViewOptions
{
    using KnowYourTurf.Core.RouteTokens;

    public interface IRouteTokenConfig
    {
        IList<RouteToken> Build(bool withoutPermissions = false);
    }

    public class SystemSupportRouteTokenList : IRouteTokenConfig
    {
        private readonly IRouteTokenBuilder _builder;

        public SystemSupportRouteTokenList(IRouteTokenBuilder routeTokenBuilder)
        {
            _builder = routeTokenBuilder;
        }

        public IList<RouteToken> Build(bool withoutPermissions = false)
        {
            _builder.WithoutPermissions(withoutPermissions);
            _builder.Url<OrthogonalController>(x => x.SystemSupportMenu()).ViewId("systemSupportMenu").End();
            
            _builder.TokenForList<UserListController>(x => x.ItemList(null)).ViewName("UserGridView").End();
            _builder.TokenForForm<UserController>(x => x.AddUpdate(null)).ViewName("UserSettingsView").End();

            _builder.TokenForList<ClientListController>(x => x.ItemList(null)).End();
            _builder.TokenForForm<ClientController>(x => x.AddUpdate(null)).End();

            _builder.TokenForList<EmailTemplateListController>(x => x.ItemList(null)).End();
            _builder.TokenForForm<EmailTemplateController>(x => x.AddUpdate(null)).End();

            _builder.TokenForList<LoginStatisticsListController>(x => x.ItemList(null)).End();

//            _builder.TokenForList<UserPermissionListController>(x => x.ItemList(null)).End();
            _builder.TokenForForm<PermissionController>(x => x.AddUpdate(null)).End();

            _builder.TokenForList<UserGroupListController>(x => x.ItemList(null)).End();
            _builder.TokenForForm<UserGroupController>(x => x.AddUpdate(null)).End();

            _builder.TokenForList<UserGroupPermissionListController>(x => x.ItemList(null)).End();

            _builder.TokenForForm<SiteConfigurationController>(x => x.AddUpdate(null)).End();

            return _builder.Items;
        }

    }
}