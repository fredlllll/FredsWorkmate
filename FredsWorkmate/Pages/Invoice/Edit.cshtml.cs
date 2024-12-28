using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Invoice
{
    public class InvoiceEdit : BaseEdit<Database.Models.Invoice>
    {
        public InvoiceEdit(DatabaseContext db) : base(db) { }
    }
}
