using System.Web.Mvc;

namespace SystemSupport.Web.Areas.Permissions
{
    public class PermissionsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Permissions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Permissions_default",
                "Permissions/{controller}/{action}/{EntityId}",
                new { action = "Index", EntityId = UrlParameter.Optional }
            );
        }
    }
}
