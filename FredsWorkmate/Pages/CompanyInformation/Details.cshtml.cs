using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.CompanyInformation
{
    public class CompanyInformationDetails : BaseDetails<Database.Models.CompanyInformation>
    {
        public CompanyInformationDetails(DatabaseContext db) : base(db) { }
    }
}
