using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace FredsWorkmate.Util
{
    public static class HtmlHelperExtensions
    {
        static readonly Func<Model, SelectListItem> ModelToSelectListItem = x => new SelectListItem { Value = x.Id, Text = x.ToString() };

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
                writer.WriteLine($"  <dt>{p.Name}</dt>");
                writer.Write("  <dd>");
                if (p.PropertyType.IsAssignableTo(typeof(Model)))
                {
                    dynamic dset = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<DatabaseContext>().GetEntityDbSet(p.PropertyType);
                    writer.WriteLine(htmlHelper.DropDownList(p.Name, Enumerable.Select(dset, ModelToSelectListItem), p.Name, null));
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

        public static void EntityTable<T>(this IHtmlHelper htmlHelper, DbSet<T> dbSet) where T : Model
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
                else if (p.PropertyType.IsGenericType(typeof(ICollection<>)))
                {
                    //TODO: how will we display navigation lists?
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
                if (!Attribute.IsDefined(p, typeof(AutoParameterAttribute)))
                {
                    writer.WriteLine($"  <dt>{p.Name}</dt>");
                    writer.Write("  <dd>");
                    if (p.PropertyType.IsAssignableTo(typeof(Model)))
                    {
                        dynamic dset = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<DatabaseContext>().GetEntityDbSet(p.PropertyType);
                        //TODO: select currently set value
                        writer.WriteLine(htmlHelper.DropDownList(p.Name, Enumerable.Select(dset, ModelToSelectListItem), p.Name, null));
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

        public static void DetailNavigation(this IHtmlHelper htmlHelper, string id)
        {
            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div>");
            writer.WriteLine($"<a href=\"../Edit/{id}\">Edit</a>");
            writer.WriteLine($"<a href=\"..\">Back to List</a>");
            writer.WriteLine("</div>");
        }
    }
}
