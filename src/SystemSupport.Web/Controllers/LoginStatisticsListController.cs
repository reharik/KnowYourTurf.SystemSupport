using System;
using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.Html;
    using CC.Core.Services;

    using FluentNHibernate.Conventions;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class LoginStatisticsListController:DCIController
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
            var gridDefinition = _grid.GetGridDefinition(url);
            var model = new ListViewModel
            {
                Title = WebLocalizationKeys.LOGIN_INFORMATION.ToString(),
                gridDef= gridDefinition,
            };
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = input.filters.IsNotEmpty()
                ? _dynamicExpressionQuery.PerformQuery<LoginStatistics>(input.filters)
                : _dynamicExpressionQuery.PerformQuery<LoginStatistics>(input.filters, x=>x.CreatedDate.Value.Date==DateTime.Now.Date);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items);
            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}