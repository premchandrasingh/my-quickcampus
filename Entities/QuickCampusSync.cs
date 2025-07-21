namespace My.QuickCampus.Entities
{
    public class QuickCampusSync
    {
        public long QuickCampusSyncId { get; set; }

        public long GradeId { get; set; }

        /// <summary>
        /// Month of the source data being synced.
        /// </summary>
        public int SourceMonth { get; set; }

        /// <summary>
        /// Year of the source data being synced.
        /// </summary>
        public int SourceYear { get; set; }

        /// <summary>
        /// The date and time when the sync was performed.
        /// </summary>

        /// <summary>
        /// The type of sync operation performed (e.g., "Homework", "Assignment").
        /// </summary>
        public string SyncType { get; set; } = string.Empty;


        public DateTime SyncDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The status of the sync operation (e.g., "Success", "Failed").
        /// </summary>
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// Any additional information or error messages related to the sync operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Grade Grade { get; set; }

    }
}
