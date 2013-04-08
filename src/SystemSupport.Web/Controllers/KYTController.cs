namespace SystemSupport.Web.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    using SystemSupport.Web.Config;
    using SystemSupport.Web.Filters;

    //    [AddUserToViewFilter, CustomAuthorize, PermissionValidation]
    [CustomAuthorize(Order = 2)]
    [AddUserToViewModelFilter(Order = 1)]
    public class KYTController : Controller
    {
        protected string RenderViewToString()
        {
            return this.RenderViewToString(null, null);
        }

        protected string RenderViewToString(string viewName)
        {
            return this.RenderViewToString(viewName, null);
        }

        protected string RenderViewToString(object model)
        {
            return this.RenderViewToString(null, model);
        }

        protected string RenderViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName)) viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                //ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(this.ControllerContext, viewName, null);
                ViewContext viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }

    public class AjaxAwareRedirectResult : RedirectResult
    {
        public AjaxAwareRedirectResult(string url)
            : base(url)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                string destinationUrl = UrlHelper.GenerateContentUrl(Url, context.HttpContext);

                JsonResult result = new JsonResult()
                {
                    Data = new { LoggedOut = true, RedirectUrl = destinationUrl }
                };
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                result.ExecuteResult(context);
            }
            else
                base.ExecuteResult(context);
        }
    }
}