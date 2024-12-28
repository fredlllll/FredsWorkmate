using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.BankInformation
{
    public class BankInformationDetails : BaseDetails<Database.Models.BankInformation>
    {
        public BankInformationDetails(DatabaseContext db) : base(db) { }
    }
}
