using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.InvoicePosition
{
    public class InvoicePositionDetails : BaseDetails<Database.Models.InvoicePosition>
    {
        public InvoicePositionDetails(DatabaseContext db) : base(db) { }
    }
}
