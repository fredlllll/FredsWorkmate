using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Note
{
    public class NoteIndex : BaseIndex<Database.Models.Note>
    {
        public NoteIndex(DatabaseContext db) : base(db) { }
    }
}
