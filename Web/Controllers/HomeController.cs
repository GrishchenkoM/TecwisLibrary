using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult NewBookView(int? index)
        {
            if(index != null)
                return View("_ManageBook", index);

            return View("_ManageBook", -1);
        }
    }
}
