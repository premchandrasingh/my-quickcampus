namespace My.QuickCampus.Entities
{
    public class Grade
    {
        public long GradeId { get; set; }

        public long StudentId { get; set; }

        /// <summary>
        /// Is Current Class - This is used to determine if this is the current class of the student.
        /// </summary>
        public bool IsCurrentClass { get; set; } = false;

        /// <summary>
        /// Lile III, IV, V, etc.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public string Section { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public Student Student { get; set; }


        public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        public virtual ICollection<QuickCampusSync> QuickCampusSyncs { get; set; } = new List<QuickCampusSync>();


    }
}
