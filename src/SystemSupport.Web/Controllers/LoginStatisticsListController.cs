using System;
using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using SystemSupport.Web.Config;

    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.Html;
    using CC.Core.Services;

    using FluentNHibernate.Conventions;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;
    using KnowYourTurf.Web.Controllers;

    public class LoginStatisticsListController : KYTController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<LoginStatistics> _grid;

        public LoginStatisticsListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IEntityListGrid<LoginStatistics> grid)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _grid = grid;
        }

        public ActionResult ItemList(ListViewModel viewModel)
        {
            var url = UrlContext.GetUrlForAction<LoginStatisticsListController>(x => x.Items(null));
            var gridDefinition = _grid.GetGridDefinition(url,viewModel.User);
            var model = new ListViewModel
            {
                _Title = WebLocalizationKeys.LOGIN_INFORMATION.ToString(),
                gridDef= gridDefinition,
            };
            return new CustomJsonResult(model);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = _dynamicExpressionQuery.PerformQuery<LoginStatistics>(input.filters,x=>x.ClientId==input.RootId);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return new CustomJsonResult(gridItemsViewModel);
        }
    }
}