namespace FredsWorkmate.Database.Models
{
    public interface INoteOwner
    {
        string Id { get; set; }
        ICollection<Note> Notes { get; set; }
    }
}
