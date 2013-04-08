namespace SystemSupport.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using CC.Core.CoreViewModelAndDTOs;

    using SystemSupport.Web.Services.ViewOptions;

    using KnowYourTurf.Core.RouteTokens;

    public class SystemSupportController : KYTController
    {
        private IRouteTokenConfig _routeTokenConfig;

        public SystemSupportController(IRouteTokenConfig routeTokenConfig)
        {
            this._routeTokenConfig = routeTokenConfig;
        }

        public ActionResult Home(ViewModel input)
        {
            var knowYourTurfViewModel = new KnowYourTurfViewModel
            {
                SerializedRoutes = this._routeTokenConfig.Build(true)
            };
            return this.View(knowYourTurfViewModel);
        }
    }

    public class KnowYourTurfViewModel : ViewModel
    {
        public IList<RouteToken> SerializedRoutes { get; set; }
    }
}