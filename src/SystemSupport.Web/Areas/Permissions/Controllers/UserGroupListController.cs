using System.Linq;
using System.Web.Mvc;

using SystemSupport.Web.Areas.Permissions.Grids;
using SystemSupport.Web.Controllers;
using SystemSupport.Web.Services.ViewOptions;

namespace SystemSupport.Web.Areas.Permissions.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.Html;
    using CC.Core.Html.Grid;
    using CC.Core.Services;
    using CC.Security.Interfaces;

    using KnowYourTurf.Core.Enums;
    using KnowYourTurf.Core.Services;
    using KnowYourTurf.Web.Controllers;

    public class UserGroupListController : KYTController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<UserGroupDto> _grid;
        private readonly IAuthorizationRepository _authorizationRepository;

        public UserGroupListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IAuthorizationRepository authorizationRepository,
            IEntityListGrid<UserGroupDto> grid)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _authorizationRepository = authorizationRepository;
            _grid = grid;
        }

        public ActionResult ItemList(ListViewModel input)
        {
            var url = UrlContext.GetUrlForAction<UserGroupListController>(x => x.Items(null));
            var gridDefinition = _grid.GetGridDefinition(url, input.User);
            var model = new ListViewModel
            {
                _Title = WebLocalizationKeys.USER_GROUPS.ToString(),
                addUpdateUrl = UrlContext.GetUrlForAction<UserGroupController>(x=>x.AddUpdate(null),AreaName.Permissions),
                gridDef= gridDefinition
            };
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var usersGroups = _authorizationRepository.GetAllUsersGroups().Select(x =>
                                                                      {
                                                                          var name = x.Name;
                                                                          var fullName = name;
                                                                          string firstToken = string.Empty;
                                                                          var offset = name.IndexOf('/', 1);

                                                                          if (name.Count(c => c == '/') == 2)
                                                                          {
                                                                              firstToken = name.Substring(1, offset-1);
                                                                              name = name.Replace("/" + firstToken + "/", "");
                                                                          }
                                                                          else if (name.Count(c => c == '/') > 2)
                                                                          {
                                                                              firstToken = name.Substring(1, name.IndexOf('/', offset + 1));
                                                                              name = name.Replace("/" + firstToken, "");
                                                                              firstToken = firstToken.Replace("/", " ");
                                                                          }

                                                                          return new UserGroupDto
                                                                                      {
                                                                                          EntityId = x.EntityId,
                                                                                          Name = name,
                                                                                          FirstToken = firstToken,
                                                                                          FullGroupName = fullName
                                                                                      };
                                                                      });
            IQueryable<UserGroupDto> items = _dynamicExpressionQuery.PerformQuery(usersGroups, input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
        }
    }

    public class UserGroupDto : IGridEnabledClass
    {
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string FirstToken { get; set; }
        public string FullGroupName { get; set; }
    }
}