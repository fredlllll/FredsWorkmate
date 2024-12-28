using FredsWorkmate.Database;
using FredsWorkmate.Pages.BasePages;

namespace FredsWorkmate.Pages.Note
{
    public class NoteEdit : BaseEdit<Database.Models.Note>
    {
        public NoteEdit(DatabaseContext db) : base(db) { }
    }
}
