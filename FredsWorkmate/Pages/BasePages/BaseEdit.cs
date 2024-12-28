using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Util;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FredsWorkmate.Pages.BasePages
{
    public class BaseEdit<T> : PageModel where T : Model
    {
        public T Entity { get; set; }

        public readonly DatabaseContext db;

#pragma warning disable CS8618
        public BaseEdit(DatabaseContext db)
#pragma warning restore CS8618
        {
            this.db = db;
        }

        public void OnGet(string id)
        {
            Entity = db.Set<T>().First(x => x.Id == id);
        }

        public void OnPostEdit(string id)
        {
            Entity = this.UpdateEntity<T>(id);
            db.SaveChanges();
        }
    }
}
