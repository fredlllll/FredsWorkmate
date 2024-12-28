using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.InvoicePosition
{
    public class InvoicePositionEdit : BaseEdit<Database.Models.InvoicePosition>
    {
        public InvoicePositionEdit(DatabaseContext db) : base(db) { }
    }
}
