using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_PropertyEditor
    {
        public static void PropertyEditor(this IHtmlHelper htmlHelper, PropertyInfo property, Model? entityInstance = null)
        {
            var writer = htmlHelper.ViewContext.Writer;
            var db = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();

            //TODO: check types
            if (entityInstance == null)
            {
                writer.WriteLine(htmlHelper.TextBox(property.Name));
            }
            else
            {
                writer.WriteLine(htmlHelper.TextBox(property.Name, property.GetValue(entityInstance)));
            }
        }
    }
}
