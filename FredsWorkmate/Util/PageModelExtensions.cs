using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Reflection;

namespace FredsWorkmate.Util
{
    public static class PageModelExtensions
    {
        public static readonly Dictionary<Type, Func<PropertyInfo, string?, object?>> typeParsers = new();

        public static T CreateEntity<T>(this PageModel page) where T : Model
        {
            DatabaseContext db = page.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();
            HttpRequest req = page.Request;
            Type t = typeof(T);

            T instance = ((T?)Activator.CreateInstance(t)) ?? throw new InvalidOperationException($"can not create instance of {t}");
            instance.Id = db.GetNewId<T>();

            foreach (var p in t.GetEditableProperties())
            {
                var value = req.Form[p.Name].First();
                if (p.PropertyType.IsAssignableTo(typeof(Model)))
                {
                    var val = db.Find(p.PropertyType, value);
                    p.SetValue(instance, val);
                }
                else if (typeParsers.TryGetValue(p.PropertyType, out var parser))
                {
                    p.SetValue(instance, parser.Invoke(p, value));
                }
                else if (p.PropertyType.IsEnum)
                {
                    if (value == null)
                    {
                        p.SetValue(instance, 0);
                    }
                    else
                    {
                        p.SetValue(instance, Enum.Parse(p.PropertyType, value));
                    }
                }
                else
                {

                    p.SetValue(instance, Convert.ChangeType(value, p.PropertyType, CultureInfo.InvariantCulture));
                }
            }
            return instance;
        }

        public static T UpdateEntity<T>(this PageModel page, string id) where T : Model
        {
            DatabaseContext db = page.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();
            HttpRequest req = page.Request;
            Type t = typeof(T);

            T instance = db.Set<T>().Where(x => x.Id == id).First();
            instance.Updated = DateTime.UtcNow;

            foreach (var p in t.GetEditableProperties())
            {
                var value = req.Form[p.Name].First();
                if (p.PropertyType.IsAssignableTo(typeof(Model)))
                {
                    var val = db.Find(p.PropertyType, value);
                    p.SetValue(instance, val);
                }
                else if (typeParsers.TryGetValue(p.PropertyType, out var parser))
                {
                    p.SetValue(instance, parser.Invoke(p, value));
                }
                else if (p.PropertyType.IsEnum)
                {
                    if (value == null)
                    {
                        p.SetValue(instance, 0);
                    }
                    else
                    {
                        p.SetValue(instance, Enum.Parse(p.PropertyType, value));
                    }
                }
                else
                {
                    p.SetValue(instance, Convert.ChangeType(value, p.PropertyType, CultureInfo.InvariantCulture));
                }
            }
            return instance;
        }

        static object? DateTimeParser(PropertyInfo p, string? value)
        {
            if (value == null)
            {
                return null;
            }
            return DateTime.Parse(value, CultureInfo.InvariantCulture);
        }

        static object? DateOnlyParser(PropertyInfo p, string? value)
        {
            if (value == null)
            {
                return null;
            }
            return DateOnly.Parse(value, CultureInfo.InvariantCulture);
        }

        static PageModelExtensions()
        {
            typeParsers[typeof(DateTime)] = DateTimeParser;
            typeParsers[typeof(DateOnly)] = DateOnlyParser;
        }
    }
}
