using System.Web.Mvc;

using SystemSupport.Web.Grids;
using SystemSupport.Web.Models;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Html;
    using CC.Core.Html.Grid;
    using CC.Core.Services;
    using CC.Security.Interfaces;

    using KnowYourTurf.Core.Config;
    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class UserListController:DCIController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<UserLoginInfo> _grid;
        private readonly IRepository _repository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public UserListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IEntityListGrid<UserLoginInfo> grid,
            IRepository repository,
            IAuthorizationRepository authorizationRepository)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _grid = grid;
            _repository = repository;
            _authorizationRepository = authorizationRepository;
        }

        public ActionResult ItemList(ListViewModel viewModel)
        {
            var addUpdateUrl = UrlContext.GetUrlForAction<UserController>(x=>x.AddUpdate(null));
            var deleteMultipleUrl = UrlContext.GetUrlForAction<UserListController>(x => x.DeleteMultiple(null));
            var redirectUrl = SiteConfig.Settings().DCIUrl;
            var url = UrlContext.GetUrlForAction<UserListController>(x => x.Items(null));
            var gridDefinition = _grid.GetGridDefinition(url);
            var model = new UserListViewModel
            {
                addUpdateUrl = addUpdateUrl,
                _Title = WebLocalizationKeys.USERS.ToString(),
                gridDef= gridDefinition,
                deleteMultipleUrl= deleteMultipleUrl,
                RedirectUrl = redirectUrl,
                getLogin = UrlContext.GetUrlForAction<UserController>(x=>x.Login(null))
            };
            model.headerButtons.Add("new");
            model.headerButtons.Add("delete");
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = _dynamicExpressionQuery.PerformQuery<UserLoginInfo>(input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items);
            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteMultiple(BulkActionViewModel input)
        {
            input.EntityIds.ForEachItem(x =>
            {
                var userLoginInfo = _repository.Find<UserLoginInfo>(x);
                if (userLoginInfo != null)
                {
                    User deleteMe=null;
                    if (userLoginInfo.User.UserLoginInfos.Count() == 1)
                    {
                        deleteMe = userLoginInfo.User;
                    }
                    _authorizationRepository.DetachUserFromAllGroups(userLoginInfo);
                    _repository.Delete(userLoginInfo);
                    if(deleteMe!=null)
                    {
                        _repository.Delete(deleteMe);
                    }
                }
            });
            _repository.Commit();
            return Json(new Notification { Success = true }, JsonRequestBehavior.AllowGet);
        }
    }

    public class UserListViewModel : ListViewModel
    {
        public string getLogin { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class UserListGridDto:IGridEnabledClass{
        public int EntityId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }

        public int LoginInfoEntityId { get; set; }
    }

}