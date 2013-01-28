using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using SystemSupport.Web.Areas.Permissions.Controllers;
using SystemSupport.Web.Areas.Permissions.Grids;
using SystemSupport.Web.Controllers;

using StructureMap;

namespace SystemSupport.Web.Services.ViewOptions
{
    using CC.Core.Html;
    using CC.Core.Utilities;

    using KnowYourTurf.Core.Enums;

    public class SystemSupportViewOptions 
    {
        public static Cache<string, IRouteToken> Items = new Cache<string, IRouteToken>()
            .Add(new RouteToken().Route<OrthogonalController>(x => x.SystemSupportMenu()).ViewId("systemSupportMenu"))
            .Add(new RouteToken().RouteForList<UserListController>(x => x.ItemList(null)).ViewName("UserGridView"))
            .Add(new RouteToken().RouteForForm<UserController>(x => x.AddUpdate(null)).ViewName("UserSettingsView")) 
            .Add(new RouteToken().RouteForList<CompanyListController>(x => x.ItemList(null)))
            .Add(new RouteToken().RouteForForm<ClientController>(x => x.AddUpdate(null)).ViewName("ClientView"))
            .Add(new RouteToken().RouteForList<PromotionListController>(x => x.ItemList(null)))
            .Add(new RouteToken().RouteForForm<PromotionController>(x => x.AddUpdate(null)).ViewName("PromotionView"))
            .Add(new RouteToken().RouteForList<PendingChargesListController>(x => x.ItemList(null)).ViewName("PendingChargesGridView"))
            .Add(new RouteToken().RouteForForm<PendingChargesController>(x => x.Display(null)))
            .Add(new RouteToken().RouteForList<EmailTemplateListController>(x => x.ItemList(null)).ViewName("EmailTemplateGridView"))
            .Add(new RouteToken().RouteForForm<EmailTemplateController>(x => x.AddUpdate(null)))
            .Add(new RouteToken().RouteForList<LoginStatisticsListController>(x => x.ItemList(null)).ViewName("LoginStatisticsGridView"))
            .Add(new RouteToken().RouteForList<UserPermissionListController>(x => x.ItemList(null), AreaName.Permissions).ViewName("UserPermissionGridView"))
            .Add(new RouteToken().RouteForForm<UserGroupController>(x => x.AddUpdate(null), AreaName.Permissions).ViewName("UserGroupView"))
            .Add(new RouteToken().RouteForList<UserGroupListController>(x => x.ItemList(null), AreaName.Permissions).ViewName("UserPermissionGridView")) // can use this view cuz it's pretty much the same
            .Add(new RouteToken().RouteForList<UserGroupPermissionListController>(x => x.ItemList(null), AreaName.Permissions).ViewName("UserGroupPermissionGridView"))
            .Add(new RouteToken().RouteForForm<PermissionController>(x => x.AddUpdate(null), AreaName.Permissions))

            .Add(new RouteToken().RouteForForm<SiteConfigurationController>(x => x.AddUpdate(null)))
            ;


            


        public static IRouteToken GetOption<CONTROLLER>(Expression<Func<CONTROLLER, object>> action, AreaName areaName = null) where CONTROLLER : Controller
        {
            var item = Items.Retrieve(UrlContext.GetUrlForAction(action, areaName));
            return (item.Operation == null || ObjectFactory.Container.GetInstance<ICheckOperationService>().GetPermissionForOperation(item.Operation))
             ? item : null;
        }

        public static IEnumerable<IRouteToken> GetAllOptions()
        {
            var checkOperationService = ObjectFactory.Container.GetInstance<ICheckOperationService>();
            var viewOptionItems = Items.GetAll().Where(x => x.Operation == null || checkOperationService.GetPermissionForOperation(x.Operation)).ToList();
            return viewOptionItems;
        }
    }

}