using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_PropertyEditor
    {
        public static readonly Dictionary<Type, Action<TextWriter, PropertyInfo, Model?>> typeEditors = new();

        public static void PropertyEditor(this IHtmlHelper htmlHelper, PropertyInfo property, Model? entityInstance = null)
        {
            var writer = htmlHelper.ViewContext.Writer;
            var db = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();
            var t = property.PropertyType;

            if (typeEditors.TryGetValue(t, out var editor))
            {
                editor(writer, property, entityInstance);
            }
            else
            {
                //Default to textbox
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

        static void DateTimeEditor(TextWriter writer, PropertyInfo property, Model? entityInstance)
        {
            string name = property.Name;
            if (entityInstance == null)
            {
                writer.WriteLine($"<input type=\"datetime-local\" name=\"{name}\" />");
            }
            else
            {
                DateTime value = (DateTime)property.GetValue(entityInstance)!;
                writer.WriteLine($"<input type=\"datetime-local\" name=\"{name}\" value=\"{value:s}\" />");
            }
        }

        static void DateOnlyEditor(TextWriter writer, PropertyInfo property, Model? entityInstance)
        {
            string name = property.Name;
            if (entityInstance == null)
            {
                writer.WriteLine($"<input type=\"date\" name=\"{name}\" />");
            }
            else
            {
                DateOnly value = (DateOnly)property.GetValue(entityInstance)!;
                writer.WriteLine($"<input type=\"date\" name=\"{name}\" value=\"{value:o}\" />");
            }
        }

        static HtmlHelper_PropertyEditor()
        {
            typeEditors[typeof(DateTime)] = DateTimeEditor;
            typeEditors[typeof(DateOnly)] = DateOnlyEditor;
        }
    }
}
