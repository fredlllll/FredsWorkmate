using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.CompanyInformation
{
    public class CompanyInformationIndex : BaseIndex<Database.Models.CompanyInformation>
    {
        public CompanyInformationIndex(DatabaseContext db) : base(db) { }
    }
}
