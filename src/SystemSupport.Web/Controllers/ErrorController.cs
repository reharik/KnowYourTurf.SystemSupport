using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    public class ErrorController:Controller  
    {
        public ActionResult Trouble()
        {
            return View("Error");
        }
    }
}