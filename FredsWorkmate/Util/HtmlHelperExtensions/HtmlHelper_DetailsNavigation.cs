using Microsoft.AspNetCore.Mvc.Rendering;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_DetailsNavigation
    {
        public static void DetailsNavigation(this IHtmlHelper htmlHelper, string id)
        {
            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div>");
            writer.WriteLine($"<a href=\"../Edit/{id}\">Edit</a>");
            writer.WriteLine($"<a href=\"..\">Back to List</a>");
            writer.WriteLine($"<form method=\"POST\" action=\"?handler=Delete\">");
            writer.WriteLine(htmlHelper.AntiForgeryToken());
            writer.WriteLine($"<input type=\"submit\" value=\"Delete\" style=\"color:red\">");
            writer.WriteLine("</form>");
            writer.WriteLine("</div>");
        }
    }
}
