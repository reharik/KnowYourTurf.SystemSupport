using System;
using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using SystemSupport.Web.Config;

    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Html;
    using CC.Core.Html.Grid;
    using CC.Core.Services;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;
    using KnowYourTurf.Web.Controllers;

    public class ClientListController : KYTController
    {
        private readonly IEntityListGrid<Client> _grid;
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IRepository _repository;
        private readonly ISessionContext _sessionContext;

        public ClientListController(IEntityListGrid<Client> grid,
            IDynamicExpressionQuery dynamicExpressionQuery,
            IRepository repository,
            ISessionContext sessionContext)
        {
            _grid = grid;
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _repository = repository;
            _sessionContext = sessionContext;
        }

        public ActionResult ItemList(ViewModel input)
        {
            _sessionContext.RemoveSessionItem("ClientId");
            var addUpdateUrl = UrlContext.GetUrlForAction<ClientController>(x => x.AddUpdate(null));
            var url = UrlContext.GetUrlForAction<ClientListController>(x=>x.Items(null));
            var gridDefinition = _grid.GetGridDefinition(url, input.User);
            var model = new ListViewModel
            {
                _Title = WebLocalizationKeys.CLIENT.ToString(),
                gridDef = gridDefinition,
                addUpdateUrl = addUpdateUrl
            };
            model.headerButtons.Add("new");
            return new CustomJsonResult(model);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = _dynamicExpressionQuery.PerformQuery<Client>(input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return new CustomJsonResult(gridItemsViewModel);
        }

        
    }
}