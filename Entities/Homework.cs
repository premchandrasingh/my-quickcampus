using System.ComponentModel.DataAnnotations.Schema;

namespace My.QuickCampus.Entities
{
    public class Homework
    {
        public long HomeworkId { get; set; }

        public long GradeId { get; set; }

        #region QuickCampus Fields
        /// <summary>
        /// Uid
        /// </summary>
        public string QuickCampusId { get; set; } = string.Empty;

        public string Title { get; set; }

        public string ClassSectionName { get; set; }


        public string SubjectName { get; set; }

        public string PostedDate { get; set; }

        public string Body { get; set; }

        /// <summary>
        /// Homework, Assignment, etc.
        /// </summary>        
        public string Type { get; set; }

        public string QuickCampusCreatedBy { get; set; }

        public string QuickCampusEditedBy { get; set; }

        public string QuickCampusCreatedDate { get; set; }

        public string QuickCampusEditedDate { get; set; }

        public string CreatedByEmployeeNo { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public string EditedByEmployeeNo { get; set; }

        public string EditedByEmployeeName { get; set; }

        [NotMapped]
        public List<MediaFile> Documents => (MediaFiles ?? new List<MediaFile>()).Where(mf => mf.MediaType == "Document").ToList();

        [NotMapped]
        public List<MediaFile> Images => (MediaFiles ?? new List<MediaFile>()).Where(mf => mf.MediaType == "Image").ToList();


        #endregion

        public List<MediaFile> MediaFiles { get; set; }


        public Grade Grade { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
