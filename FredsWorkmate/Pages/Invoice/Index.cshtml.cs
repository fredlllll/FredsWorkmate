using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Invoice
{
    public class InvoiceIndex : BaseIndex<Database.Models.Invoice>
    {
        public InvoiceIndex(DatabaseContext db) : base(db) { }
    }
}
