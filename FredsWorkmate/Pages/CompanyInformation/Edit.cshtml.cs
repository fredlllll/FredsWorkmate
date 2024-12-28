using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.CompanyInformation
{
    public class CompanyInformationEdit : BaseEdit<Database.Models.CompanyInformation>
    {
        public CompanyInformationEdit(DatabaseContext db) : base(db) { }
    }
}
