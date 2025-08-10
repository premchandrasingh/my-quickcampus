namespace My.QuickCampus.Entities
{
    public class MediaFile
    {
        public long MediaFileId { get; set; }

        public long? AssignmentId { get; set; }

        public long? HomeworkId { get; set; }

        /// <summary>
        /// Image, Document, 
        /// </summary>
        public string MediaType { get; set; } = "Document";

        public string FileExtension { get; set; } = string.Empty;

        public string QuickCampusFileName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        
        public string FilePath { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Homework Homework { get; set; }

        public Assignment Assignment { get; set; }
        

    }
}
