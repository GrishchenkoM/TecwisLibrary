using System.Text;
using System.Web.Mvc;

namespace Web.Models
{
    public static class PagingHelpers
    {
        public static string PageLinksHtml(int totalPages, int pageNumber, string func)
        {
            var result = new StringBuilder();
            try
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("onclick", func + "(" + i + ")");
                    tag.InnerHtml = i.ToString();

                    if (i == pageNumber)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-primary");
                    }
                    tag.AddCssClass("btn btn-default");
                    result.Append(tag);
                }
            }
            catch { }

            return result.ToString();
        }
    }
}