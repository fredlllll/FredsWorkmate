using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.TrackedTime
{
    public class TrackedTimeEdit : BaseEdit<Database.Models.TrackedTime>
    {
        public TrackedTimeEdit(DatabaseContext db) : base(db) { }
    }
}
