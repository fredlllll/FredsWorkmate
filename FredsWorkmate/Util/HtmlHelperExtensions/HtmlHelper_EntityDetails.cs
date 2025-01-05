using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_EntityDetails
    {
        public static void EntityDetails<T>(this IHtmlHelper htmlHelper, T entity) where T : Model
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            var t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            List<PropertyInfo> properties = HtmlHelper_Shared.GetSortedDisplayProperties(t);

            writer.WriteLine("<dl>");
            foreach (var p in properties)
            {
                if (p.PropertyType.IsGenericType(typeof(ICollection<>)))
                {
                    continue; // lists dont make sense in details
                }
                writer.WriteLine($"  <dt>{p.Name}</dt>");
                writer.Write("  <dd>");
                var value = p.GetValue(entity);
                if (p.PropertyType.IsAssignableTo(typeof(Model)))
                {
                    Model? val = (Model?)value;
                    if (val != null)
                    {
                        writer.WriteLine($"<a href=\"/{p.PropertyType.Name}/Details/{val.Id}\">{val}</a>");
                    }
                    else
                    {
                        writer.Write("NULL");
                    }
                }
                else
                {
                    writer.WriteLine(value);
                }
                writer.WriteLine("</dd>");
            }
            writer.WriteLine("</dl>");
        }
    }
}
