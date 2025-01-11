using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_EntityEditor
    {
        public static void EntityEditor<T>(this IHtmlHelper htmlHelper, T entity) where T : Model
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            var t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine("<form method=\"POST\" action=\"?handler=Edit\">");
            writer.WriteLine(htmlHelper.AntiForgeryToken());
            writer.WriteLine("<dl>");

            foreach (var p in t.GetProperties())
            {
                if (p.PropertyType.IsGenericType(typeof(ICollection<>)))
                {
                    continue; // lists dont make sense in edit
                }
                if (!Attribute.IsDefined(p, typeof(AutoParameterAttribute)))
                {
                    writer.WriteLine($"  <dt>{p.Name}</dt>");
                    writer.Write("  <dd>");
                    if (p.PropertyType.IsAssignableTo(typeof(Model)))
                    {
                        htmlHelper.PropertySelect(p, (Model?)p.GetValue(entity));
                    }
                    else
                    {
                        htmlHelper.PropertyEditor(p, entity);
                    }
                    writer.WriteLine("</dd>");
                }
            }

            writer.WriteLine("</dl>");
            writer.WriteLine($"<input type=\"submit\" value=\"Update\">");
            writer.WriteLine("</form>");
        }
    }
}
