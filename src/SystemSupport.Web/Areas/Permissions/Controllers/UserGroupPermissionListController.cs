using System.Linq;
using System.Web.Mvc;

using SystemSupport.Web.Areas.Permissions.Grids;
using SystemSupport.Web.Controllers;
using SystemSupport.Web.Services.ViewOptions;

using StructureMap;

namespace SystemSupport.Web.Areas.Permissions.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.Html;
    using CC.Core.Services;
    using CC.Security.Interfaces;

    using KnowYourTurf.Core.Enums;
    using KnowYourTurf.Core.Services;

    public class UserGroupPermissionListController:DCIController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<PermissionDto> _grid;
        private readonly IPermissionsService _permissionsService;
        private readonly IAuthorizationRepository _authorizationRepository;

        public UserGroupPermissionListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IPermissionsService permissionsService,
            IAuthorizationRepository authorizationRepository)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _grid = ObjectFactory.Container.GetInstance<IEntityListGrid<PermissionDto>>("group");
            _permissionsService = permissionsService;
            _authorizationRepository = authorizationRepository;
        }

        public ActionResult ItemList(UserGroupViewModel input)
        {
            var param = (input.EntityId > 0) ? "?ParentId=" + input.EntityId : "";
            var url = UrlContext.GetUrlForAction<UserGroupPermissionListController>(x => x.Items(null)) + param;
            var gridDefinition = _grid.GetGridDefinition(url, input.User);
            var usersGroup = _authorizationRepository.GetUsersGroupById(input.EntityId);
            var model = new SecurityListViewModel
            {
                _Title = WebLocalizationKeys.PERMISSIONS_FOR.ToFormat(usersGroup.Name),
                gridDef= gridDefinition,
                addUpdateUrl = UrlContext.GetUrlForAction<PermissionController>(x => x.AddUpdate(null), AreaName.Permissions),
                deleteMultipleUrl = UrlContext.GetUrlForAction<PermissionController>(x=>x.DeleteMultiple(null),AreaName.Permissions),
                headerButtons = new[] {"new","delete"}.ToList(),
                EntityId = input.EntityId,
                ParentId = usersGroup.EntityId

            };
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var usersGroup = _authorizationRepository.GetUsersGroupById(input.ParentId);
            var permissionDtos =
                _permissionsService.GetPermissionsFor(usersGroup).Select(x =>
                                                                      {
                                                                          var opName = x.Operation.Name;
                                                                          string firstToken = string.Empty;
                                                                          var offset = opName.IndexOf('/',1);
                                                                          
                                                                          if(opName.Count(c=>c=='/') == 2)
                                                                          {
                                                                              firstToken = opName.Substring(1, offset-1);
                                                                              opName = opName.Replace("/" + firstToken +"/", "");
                                                                          }
                                                                          else if (opName.Count(c => c == '/') > 2)
                                                                          {
                                                                              firstToken = opName.Substring(1,opName.IndexOf('/', offset + 1));
                                                                              opName = opName.Replace("/"+firstToken , "");
                                                                              firstToken = firstToken.Replace("/", " ");
                                                                          }

                                                                          return new PermissionDto
                                                                                      {
                                                                                          State = x.Allow ? "Allow" : "Deny",
                                                                                          Level = x.Level,
                                                                                          Operation = opName,
                                                                                          EntityId = x.EntityId,
                                                                                          FirstToken = firstToken,
                                                                                          UserGroup = usersGroup.Name,
                                                                                          UserGroupEntityId = usersGroup.EntityId
                                                                                      };
                                                                      });
            IQueryable<PermissionDto> items = _dynamicExpressionQuery.PerformQuery(permissionDtos, input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}