using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Project
{
    public class ProjectDetails : BaseDetails<Database.Models.Project>
    {
        public ProjectDetails(DatabaseContext db) : base(db) { }
    }
}
