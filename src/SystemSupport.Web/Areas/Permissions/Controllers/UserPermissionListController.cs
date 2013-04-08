using System.Linq;
using System.Web.Mvc;

using SystemSupport.Web.Areas.Permissions.Grids;
using SystemSupport.Web.Controllers;
using SystemSupport.Web.Grids;
using SystemSupport.Web.Services.ViewOptions;

namespace SystemSupport.Web.Areas.Permissions.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Enumerations;
    using CC.Core.Html;
    using CC.Core.Html.Grid;
    using CC.Core.Services;
    using CC.Security.Interfaces;

    using KnowYourTurf.Core.Domain;

//    public class UserPermissionListController:DCIController
//    {
//        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
//        private readonly IEntityListGrid<PermissionDto> _grid;
//        private readonly IPermissionsService _permissionsService;
//        private readonly IRepository _repository;
//
//        public UserPermissionListController(IDynamicExpressionQuery dynamicExpressionQuery,
//            IEntityListGrid<PermissionDto> grid,
//            IPermissionsService permissionsService,
//            IRepository repository)
//        {
//            _dynamicExpressionQuery = dynamicExpressionQuery;
//            _grid = grid;
//            _permissionsService = permissionsService;
//            _repository = repository;
//        }
//
//        public ActionResult ItemList(ListViewModel input)
//        {
//            var uli = _repository.Find<UserLoginInfo>(input.EntityId);
//            var url = UrlContext.GetUrlForAction<UserPermissionListController>(x => x.Items(null)) + "?ParentId=" + input.EntityId;
//            var gridDefinition = _grid.GetGridDefinition(url);
//            var model = new UserPermissionListViewModel
//            {
//                Title = WebLocalizationKeys.PERMISSIONS_FOR.ToFormat(uli.User.FullNameFNF),
//                gridDef= gridDefinition,
//                addUpdateRoute = SystemSupportViewOptions.GetOption<PermissionController>(x=>x.AddUpdate(null),AreaName.Permissions).route,
//                addUpdateUserGroupRoute = SystemSupportViewOptions.GetOption<UserGroupController>(x => x.AddUpdate(null), AreaName.Permissions).route,
//                deleteMultipleUrl = UrlContext.GetUrlForAction<PermissionController>(x=>x.DeleteMultiple(null),AreaName.Permissions),
//                RootId = uli.EntityId,
//                headerButtons = new[] { "new", "delete" }.ToList()
//            };
//            return Json(model,JsonRequestBehavior.AllowGet);
//        }
//
//        public JsonResult Items(GridItemsRequestModel input)
//        {
//            var uli = _repository.Find<UserLoginInfo>(input.ParentId);
//            var permissionDtos =
//                _permissionsService.GetPermissionsFor(uli).Select(x =>
//                                                                      {
//                                                                          var opName = x.Operation.Name;
//                                                                          string firstToken = string.Empty;
//                                                                          var offset = opName.IndexOf('/',1);
//                                                                          
//                                                                          if(opName.Count(c=>c=='/') == 2)
//                                                                          {
//                                                                              firstToken = opName.Substring(1, offset-1);
//                                                                              opName = opName.Replace("/" + firstToken +"/", "");
//                                                                          }
//                                                                          else if (opName.Count(c => c == '/') > 2)
//                                                                          {
//                                                                              firstToken = opName.Substring(1,opName.IndexOf('/', offset + 1));
//                                                                              opName = opName.Replace("/"+firstToken , "");
//                                                                              firstToken = firstToken.Replace("/", " ");
//                                                                          }
//
//                                                                          return new PermissionDto
//                                                                                      {
//                                                                                          State = x.Allow ? "Allow" : "Deny",
//                                                                                          OnUser = x.User != null,
//                                                                                          UserGroup = x.UsersGroup != null ? x.UsersGroup.Name : "",
//                                                                                          UserGroupEntityId = x.UsersGroup != null ? x.UsersGroup.EntityId : 0,
//                                                                                          UliId = x.User != null?uli.EntityId:0,
//                                                                                          Level = x.Level,
//                                                                                          Operation = opName,
//                                                                                          FullOperationName = x.Operation.Name,
//                                                                                          EntityId = x.EntityId,
//                                                                                          FirstToken = firstToken
//                                                                                      };
//                                                                      });
//            IQueryable<PermissionDto> items = _dynamicExpressionQuery.PerformQueryWithItems(permissionDtos, input.filters);
//            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items);
//            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
//        }
//    }

    public class SecurityListViewModel : ListViewModel
    {
        public string EntityName { get; set; }
        public string ParentName { get; set; }
    }

    public class UserPermissionListViewModel:SecurityListViewModel
    {
        public string addUpdateUserGroupRoute { get; set; }
    }

    public class PermissionDto : IGridEnabledClass
    {
        public int EntityId { get; set; }
        public string FirstToken { get; set; }
        public string State { get; set; }
        public bool OnUser{ get; set; }
        public string UserGroup { get; set; }
        public string Operation{ get; set; }
        public int Level { get; set; }
        public string FullOperationName { get; set; }

        public int UserGroupEntityId { get; set; }
        public int UliId { get; set; }
    }
}