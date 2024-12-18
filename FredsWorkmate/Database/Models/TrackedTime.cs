namespace FredsWorkmate.Database.Models
{
    public class TrackedTime : Model
    {
        public required Project Project { get; set; }
        public required double Hours { get; set; }
        public required DateOnly Date { get; set; }
    }
}
