using System;
using System.Web.Mvc;
using SystemSupport.Web.Models;
using SystemSupport.Web.Config;
using CC.Core;
using CC.Core.CoreViewModelAndDTOs;
using CC.Core.DomainTools;
using CC.Core.Html;
using CC.Core.Html.Grid;
using CC.Core.Services;
using CC.Security.Interfaces;

using KnowYourTurf.Core.Config;
using KnowYourTurf.Core.Domain;
using KnowYourTurf.Core.Services;

namespace SystemSupport.Web.Controllers
{
    

    public class FieldListController : KYTController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<Field> _grid;
        private readonly IRepository _repository;

        public FieldListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IEntityListGrid<Field> grid,
            IRepository repository)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _grid = grid;
            _repository = repository;
        }

        public ActionResult ItemList(ListViewModel input)
        {
            var deleteMultipleUrl = UrlContext.GetUrlForAction<FieldListController>(x => x.DeleteMultiple(null));
            var url = UrlContext.GetUrlForAction<FieldListController>(x => x.Items(null)) + "?RootId=" + input.RootId;
            var gridDefinition = _grid.GetGridDefinition(url, input.User);
            var model = new ListViewModel
            {
                _Title = WebLocalizationKeys.FIELDS.ToString(),
                gridDef= gridDefinition,
                deleteMultipleUrl= deleteMultipleUrl,
            };
            model.headerButtons.Add("new");
            model.headerButtons.Add("delete");
            return new CustomJsonResult(model);
        }

        public JsonResult Items(GridItemsRequestModel input)
        {
            var category = _repository.Find<Site>(input.RootId);
            var items = _dynamicExpressionQuery.PerformQuery(category.Fields, input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return new CustomJsonResult(gridItemsViewModel);
        }
        public JsonResult DeleteMultiple(BulkActionViewModel input)
        {
            throw new NotImplementedException();
//            input.EntityIds.ForEachItem(x =>
//            {
//                var field = _repository.Find<Field>(x);
//                if (field != null)
//                {
//                    _authorizationRepository.DetachFieldFromAllGroups(field);
//                    _repository.Delete(field);
//                }
//            });
//            _repository.Commit();
//            return new CustomJsonResult(new Notification { Success = true });
            return null;
        }
    }
}