namespace My.QuickCampus.Entities
{
    public class Student
    {
        public long StudentId { get; set; }

        /// <summary>
        /// This id is important and will be used to make QuickCampus api call
        /// </summary>
        public string QuickCampusId { get; set; }

        /// <summary>
        /// Also known as Addimission number
        /// </summary>
        public int ScholarNumber { get; set; } 

        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
