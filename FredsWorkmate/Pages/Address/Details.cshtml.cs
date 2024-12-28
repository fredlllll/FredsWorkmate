using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Address
{
    public class AddressDetails : BaseDetails<Database.Models.Address>
    {
        public AddressDetails(DatabaseContext db) : base(db) { }
    }
}
