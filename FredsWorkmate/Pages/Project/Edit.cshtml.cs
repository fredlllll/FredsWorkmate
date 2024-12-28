using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Project
{
    public class ProjectEdit : BaseEdit<Database.Models.Project>
    {
        public ProjectEdit(DatabaseContext db) : base(db) { }
    }
}
