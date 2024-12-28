using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Project
{
    public class ProjectIndex : BaseIndex<Database.Models.Project>
    {
        public ProjectIndex(DatabaseContext db) : base(db) { }
    }
}
