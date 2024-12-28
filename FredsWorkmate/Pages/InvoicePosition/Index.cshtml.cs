using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.InvoicePosition
{
    public class InvoicePositionIndex : BaseIndex<Database.Models.InvoicePosition>
    {
        public InvoicePositionIndex(DatabaseContext db) : base(db) { }
    }
}
