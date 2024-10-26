

namespace Project.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string ?FileName { get; set; }
        public string ?FilePath { get; set; }
        public int SubjectId { get; set; }
        public Subject ?Subject { get; set; }
    }
}
