using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class Customer : Model, INoteOwner
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required Address Address { get; set; }
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
