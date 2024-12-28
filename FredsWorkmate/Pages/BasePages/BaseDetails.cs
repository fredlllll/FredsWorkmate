using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FredsWorkmate.Pages.BasePages
{
    public class BaseDetails<T> : PageModel where T : Model
    {
        public T Entity { get; set; }

        public readonly DatabaseContext db;

#pragma warning disable CS8618
        public BaseDetails(DatabaseContext db)
#pragma warning restore CS8618
        {
            this.db = db;
        }

        public void OnGet(string id)
        {
            var set = db.Set<T>();
            Entity = set.Find(id) ?? throw new Exception("not found");
            foreach (var reference in set.Entry(Entity).References)
            {
                reference.Load();
            }
        }
    }
}
