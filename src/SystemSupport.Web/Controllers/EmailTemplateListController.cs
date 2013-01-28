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

    public class EmailTemplateListController:DCIController
    {
        private readonly IEntityListGrid<EmailTemplate> _grid;
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IRepository _repository;

        public EmailTemplateListController(IEntityListGrid<EmailTemplate> grid,
            IDynamicExpressionQuery dynamicExpressionQuery,
            IRepository repository)
        {
            _grid = grid;
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _repository = repository;
        }

        public ActionResult ItemList(ViewModel input)
        {
            var url = UrlContext.GetUrlForAction<EmailTemplateListController>(x=>x.Items(null))+"/"+input.EntityId;
            var gridDefinition = _grid.GetGridDefinition(url);
            var model = new ListViewModel
            {
                _Title = WebLocalizationKeys.EMAIL_TEMPLATE.ToString(),
                gridDef = gridDefinition,
                ParentId = input.EntityId
            };
            model.headerButtons.Add("new");
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var items = _dynamicExpressionQuery.PerformQuery((Grid<EmailTemplate>)_grid, input.filters, x=>x.CompanyId==input.EntityId);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items);
            return Json(gridItemsViewModel, JsonRequestBehavior.AllowGet);
        }

        
    }
}