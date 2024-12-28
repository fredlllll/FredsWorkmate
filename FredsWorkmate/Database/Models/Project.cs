namespace FredsWorkmate.Database.Models
{
    public class Project : Model, INoteOwner
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Customer? Customer { get; set; }
        public required ICollection<Note> Notes { get; set; }
    }
}
