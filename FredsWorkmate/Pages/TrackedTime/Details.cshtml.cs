using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.TrackedTime
{
    public class TrackedTimeDetails : BaseDetails<Database.Models.TrackedTime>
    {
        public TrackedTimeDetails(DatabaseContext db) : base(db) { }
    }
}
