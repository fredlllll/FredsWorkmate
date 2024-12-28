using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.BankInformation
{
    public class BankInformationEdit : BaseEdit<Database.Models.BankInformation>
    {
        public BankInformationEdit(DatabaseContext db) : base(db) { }
    }
}
