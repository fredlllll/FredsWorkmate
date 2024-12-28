using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Invoice
{
    public class InvoiceDetails : BaseDetails<Database.Models.Invoice>
    {
        public InvoiceDetails(DatabaseContext db) : base(db) { }
    }
}
