using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Note
{
    public class NoteDetails : BaseDetails<Database.Models.Note>
    {
        public NoteDetails(DatabaseContext db) : base(db) { }
    }
}
