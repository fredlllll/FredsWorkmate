using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Address
{
    public class AddressIndex : BaseIndex<Database.Models.Address>
    {
        public AddressIndex(DatabaseContext db) : base(db) { }
    }
}
