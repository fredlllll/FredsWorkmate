using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.TrackedTime
{
    public class TrackedTimeIndex : BaseIndex<Database.Models.TrackedTime>
    {
        public TrackedTimeIndex(DatabaseContext db) : base(db) { }
    }
}
