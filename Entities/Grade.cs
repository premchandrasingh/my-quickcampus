namespace My.QuickCampus.Entities
{
    public class Grade
    {
        public long GradeId { get; set; }

        public long StudentId { get; set; }

        /// <summary>
        /// Lile III, IV, V, etc.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Section { get; set; } = string.Empty;

        public Student Student { get; set; }


        public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    }
}
