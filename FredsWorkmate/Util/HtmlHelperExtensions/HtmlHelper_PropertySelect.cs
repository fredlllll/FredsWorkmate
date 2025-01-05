using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_PropertySelect
    {
        public static void PropertySelect(this IHtmlHelper htmlHelper, PropertyInfo property, Model? selectedValue = null)
        {
            var writer = htmlHelper.ViewContext.Writer;
            var db = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();

            writer.WriteLine($"<select name=\"{property.Name}\">");
            if (property.IsNullable())
            {
                writer.WriteLine($"<option value=\"\">Null ({property.PropertyType.Name})</option>");
            }
            else
            {
                writer.WriteLine($"<option value=\"\">Not selected, not nullable ({property.PropertyType.Name})</option>");
            }
            var dbSet = db.GetEntityDbSet(property.PropertyType);

            foreach (Model m in dbSet)
            {
                if (m.Id == selectedValue?.Id)
                {
                    writer.WriteLine($"<option selected value=\"{m.Id}\">{m}</option>");
                }
                else
                {
                    writer.WriteLine($"<option value=\"{m.Id}\">{m}</option>");
                }
            }
            writer.WriteLine("</select>");
        }
    }
}
