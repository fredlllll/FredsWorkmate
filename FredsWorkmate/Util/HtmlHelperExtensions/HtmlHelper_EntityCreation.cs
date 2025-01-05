using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_EntityCreation
    {
        public static void EntityCreationForm<T>(this IHtmlHelper htmlHelper) where T : Model
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            var t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine($"<h3>Create New {t.Name}</h3>");
            writer.WriteLine("<form method=\"POST\" action=\"?handler=Create\">");
            writer.WriteLine(htmlHelper.AntiForgeryToken());
            writer.WriteLine("<dl>");

            foreach (var p in t.GetEditableProperties())
            {
                if (p.PropertyType.IsGenericType(typeof(ICollection<>)))
                {
                    continue; // lists dont make sense in create
                }
                writer.WriteLine($"  <dt>{p.Name}</dt>");
                writer.Write("  <dd>");
                if (p.PropertyType.IsAssignableTo(typeof(Model)))
                {
                    htmlHelper.PropertySelect(p);
                }
                else
                {
                    writer.WriteLine(htmlHelper.TextBox(p.Name));
                }
                writer.WriteLine("</dd>");
            }

            writer.WriteLine("</dl>");
            writer.WriteLine($"<input type=\"submit\" value=\"Create New {t.Name}\">");
            writer.WriteLine("</form>");
        }
    }
}
