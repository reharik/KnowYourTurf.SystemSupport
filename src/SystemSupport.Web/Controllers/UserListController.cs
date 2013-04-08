using System.Web.Mvc;

using SystemSupport.Web.Grids;
using SystemSupport.Web.Models;

namespace SystemSupport.Web.Controllers
{
    using SystemSupport.Web.Config;

    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Html;
    using CC.Core.Html.Grid;
    using CC.Core.Services;
    using CC.Security.Interfaces;

    using KnowYourTurf.Core.Config;
    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    using CC.Core;

    using KnowYourTurf.Web.Controllers;

    public class UserListController : KYTController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<User> _grid;
        private readonly IRepository _repository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public UserListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IEntityListGrid<User> grid,
            IRepository repository,
            IAuthorizationRepository authorizationRepository)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _grid = grid;
            _repository = repository;
            _authorizationRepository = authorizationRepository;
        }

        public ActionResult ItemList(ListViewModel input)
        {
            var addUpdateUrl = UrlContext.GetUrlForAction<UserController>(x=>x.AddUpdate(null));
            var deleteMultipleUrl = UrlContext.GetUrlForAction<UserListController>(x => x.DeleteMultiple(null));
            var redirectUrl = SiteConfig.Config.WebSiteRoot;
            var url = UrlContext.GetUrlForAction<UserListController>(x => x.Items(null));
            var gridDefinition = _grid.GetGridDefinition(url, input.User);
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
            return new CustomJsonResult(model);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = _dynamicExpressionQuery.PerformQuery<User>(input.filters,x=>x.ClientId==input.RootId);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return new CustomJsonResult(gridItemsViewModel);
        }
        public JsonResult DeleteMultiple(BulkActionViewModel input)
        {
            input.EntityIds.ForEachItem(x =>
            {
                var user = _repository.Find<User>(x);
                if (user != null)
                {
                    _authorizationRepository.DetachUserFromAllGroups(user);
                    _repository.Delete(user);
                }
            });
            _repository.Commit();
            return new CustomJsonResult(new Notification { Success = true });
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