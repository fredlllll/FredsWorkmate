using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Customer
{
    public class CustomerDetails : BaseDetails<Database.Models.Customer>
    {
        public CustomerDetails(DatabaseContext db) : base(db) { }
    }
}
