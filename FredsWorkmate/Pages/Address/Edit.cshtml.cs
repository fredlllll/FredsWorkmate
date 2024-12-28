using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Address
{
    public class AddressEdit : BaseEdit<Database.Models.Address>
    {
        public AddressEdit(DatabaseContext db) : base(db) { }
    }
}
