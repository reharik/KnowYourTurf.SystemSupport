using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Html;
    using CC.Core.Html.Grid;
    using CC.Core.Services;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class CompanyListController:DCIController
    {
        private readonly IEntityListGrid<Company> _grid;
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IRepository _repository;

        public CompanyListController(IEntityListGrid<Company> grid,
            IDynamicExpressionQuery dynamicExpressionQuery,
            IRepository repository)
        {
            _grid = grid;
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _repository = repository;
        }

        public ActionResult ItemList(ViewModel input)
        {
            var addUpdateUrl = UrlContext.GetUrlForAction<CompanyController>(x => x.AddUpdate(null));
            var url = UrlContext.GetUrlForAction<CompanyListController>(x=>x.Items(null));
            var gridDefinition = _grid.GetGridDefinition(url);
            var model = new ListViewModel
            {
                Title = WebLocalizationKeys.CLIENT.ToString(),
                gridDef = gridDefinition,
                addUpdateUrl = addUpdateUrl
            };
            model.headerButtons.Add("new");
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = _dynamicExpressionQuery.PerformQuery((Grid<Company>)_grid, input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items);
            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
        }

        
    }
}