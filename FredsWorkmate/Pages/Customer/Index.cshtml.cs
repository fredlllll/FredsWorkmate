using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Customer
{
    public class CustomerIndex : BaseIndex<Database.Models.Customer>
    {
        public CustomerIndex(DatabaseContext db) : base(db) { }
    }
}
