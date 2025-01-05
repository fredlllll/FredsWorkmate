using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_EntityTable
    {
        public static void EntityTable<T>(this IHtmlHelper htmlHelper, IEnumerable<T> dbSet) where T : Model
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            var t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            List<PropertyInfo> properties = HtmlHelper_Shared.GetSortedDisplayProperties(t);

            writer.WriteLine("<table>");
            writer.WriteLine("  <tr>");

            foreach (var p in properties)
            {
                writer.WriteLine($"    <th>{p.Name}</th>");
            }

            writer.WriteLine("  </tr>");

            foreach (T item in dbSet)
            {
                writer.WriteLine("  <tr>");
                foreach (var p in properties)
                {
                    if (p.PropertyType.IsGenericType(typeof(ICollection<>)))
                    {
                        continue; // lists dont make sense in table
                    }
                    if (p.Name == "Id")
                    {
                        writer.WriteLine($"    <td><a href=\"{htmlHelper.ViewContext.HttpContext.Request.Path.Value?.TrimEnd('/')}/Details/{item.Id}\">{p.GetValue(item)}</a></td>");
                    }
                    else
                    {
                        writer.WriteLine($"    <td>{p.GetValue(item)}</td>");
                    }
                }
                writer.WriteLine("  </tr>");
            }

            writer.WriteLine("</table>");
        }
    }
}
