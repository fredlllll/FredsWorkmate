using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelperExtensions
    {
        static List<PropertyInfo> GetSortedDisplayProperties(Type t)
        {
            List<PropertyInfo> properties = new(t.GetProperties());
            int index = properties.FindIndex(x => x.Name == nameof(Model.Id));
            properties.MoveItem(index, 0);
            index = properties.FindIndex(x => x.Name == nameof(Model.Created));
            properties.MoveItem(index, properties.Count);
            index = properties.FindIndex(x => x.Name == nameof(Model.Updated));
            properties.MoveItem(index, properties.Count);
            return properties;
        }

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

        public static void EntityTable<T>(this IHtmlHelper htmlHelper, IEnumerable<T> dbSet) where T : Model
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            var t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            List<PropertyInfo> properties = GetSortedDisplayProperties(t);

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

        public static void EntityDetails<T>(this IHtmlHelper htmlHelper, T entity) where T : Model
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            var t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            List<PropertyInfo> properties = GetSortedDisplayProperties(t);

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
                        writer.WriteLine(htmlHelper.TextBox(p.Name, p.GetValue(entity)));
                    }
                    writer.WriteLine("</dd>");
                }
            }

            writer.WriteLine("</dl>");
            writer.WriteLine($"<input type=\"submit\" value=\"Update\">");
            writer.WriteLine("</form>");
        }

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

        public static void EnumSelect<T>(this IHtmlHelper htmlHelper, string name, T? selected = default) where T : struct, Enum
        {
            Type t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine($"<select name=\"{name}\">");
            writer.WriteLine($"<option value=\"\">Not selected ({t.Name})</option>");
            var values = Enum.GetValues<T>();
            foreach (T e in values)
            {
                if (selected != null && Equals(selected.Value, e))
                {
                    writer.WriteLine($"<option selected value=\"{e}\">{e}</option>");
                }
                else
                {
                    writer.WriteLine($"<option value=\"{e}\">{e}</option>");
                }
            }
            writer.WriteLine("</select>");
        }
    }
}
