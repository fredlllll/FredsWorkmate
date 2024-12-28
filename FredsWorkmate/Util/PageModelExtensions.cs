using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FredsWorkmate.Util
{
    public static class PageModelExtensions
    {
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
                else
                {
                    p.SetValue(instance, Convert.ChangeType(value, p.PropertyType, CultureInfo.InvariantCulture));
                }
            }
            return instance;
        }
    }
}
