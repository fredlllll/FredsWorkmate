using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.BankInformation
{
    public class BankInformationIndex : BaseIndex<Database.Models.BankInformation>
    {
        public BankInformationIndex(DatabaseContext db) : base(db) { }
    }
}
