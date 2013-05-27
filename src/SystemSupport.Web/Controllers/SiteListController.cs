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
    

    public class SiteListController : KYTController
    {
        private readonly IDynamicExpressionQuery _dynamicExpressionQuery;
        private readonly IEntityListGrid<Site> _grid;

        public SiteListController(IDynamicExpressionQuery dynamicExpressionQuery,
            IEntityListGrid<Site> grid)
        {
            _dynamicExpressionQuery = dynamicExpressionQuery;
            _grid = grid;
        }

        public ActionResult ItemList(ListViewModel input)
        {
            var deleteMultipleUrl = UrlContext.GetUrlForAction<SiteListController>(x => x.DeleteMultiple(null));
            var url = UrlContext.GetUrlForAction<SiteListController>(x => x.Items(null));
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
            var items = _dynamicExpressionQuery.PerformQuery<Site>(input.filters);
            var gridItemsViewModel = _grid.GetGridItemsViewModel(input.PageSortFilter, items, input.User);
            return new CustomJsonResult(gridItemsViewModel);
        }
        public JsonResult DeleteMultiple(BulkActionViewModel input)
        {
            throw new NotImplementedException();
//            input.EntityIds.ForEachItem(x =>
//            {
//                var site = _repository.Find<Site>(x);
//                if (site != null)
//                {
//                    _authorizationRepository.DetachSiteFromAllGroups(site);
//                    _repository.Delete(site);
//                }
//            });
//            _repository.Commit();
//            return new CustomJsonResult(new Notification { Success = true });
            return null;
        }
    }
}