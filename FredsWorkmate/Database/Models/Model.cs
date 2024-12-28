using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class Model
    {
        public Model()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
        [AutoParameter]
        public required string Id { get; set; }
        [AutoParameter]
        public DateTime Created { get; set; }
        [AutoParameter]
        public DateTime Updated { get; set; }
    }
}
