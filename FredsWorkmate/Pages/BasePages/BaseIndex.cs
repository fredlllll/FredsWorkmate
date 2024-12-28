using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Util;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FredsWorkmate.Pages.BasePages
{
    public class BaseIndex<T> : PageModel where T : Model
    {
        public readonly DatabaseContext db;

        public BaseIndex(DatabaseContext db)
        {
            this.db = db;
        }

        public void OnPostCreate()
        {
            var entity = this.CreateEntity<T>();
            db.Add(entity);
            db.SaveChanges();

            LocalRedirect($"{this.HttpContext.Request.Path.Value?.TrimEnd('/')}/Details/{entity.Id}");
        }
    }
}
