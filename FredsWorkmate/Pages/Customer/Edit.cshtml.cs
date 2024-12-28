using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Customer
{
    public class CustomerEdit : BaseEdit<Database.Models.Customer>
    {
        public CustomerEdit(DatabaseContext db) : base(db) { }
    }
}
