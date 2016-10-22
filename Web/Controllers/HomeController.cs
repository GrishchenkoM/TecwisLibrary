using System.Web;
using System.Web.Mvc;
using Web.Models;

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

        public ActionResult NewAuthorView(int? index)
        {
            if (index != null)
                return View("_ManageAuthor", index);

            return View("_ManageAuthor", -1);
        }

        public string PageLinksForBookTab(int totalPages, int pageNumber)
        {
            var result = PagingHelpers.PageLinksHtml(totalPages, pageNumber, "UpdateBooksContentWithPage");
            
            return new HtmlString(result).ToHtmlString();
        }
        public string PageLinksForAuthorTab(int totalPages, int pageNumber)
        {
            var result = PagingHelpers.PageLinksHtml(totalPages, pageNumber, "UpdateAuthorsContentWithPage");

            return new HtmlString(result).ToHtmlString();
        }
    }
}
