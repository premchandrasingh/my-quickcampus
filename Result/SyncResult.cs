using My.QuickCampus.QuickCampus;

namespace My.QuickCampus.Result
{
    public class SyncResult
    {
        /// <summary>
        /// Indicates whether the sync operation was successful.
        /// </summary>
        public bool IsSuccess { get; private set; }


        public FailedType ErrorType { get; private set; }


        public QuickCampusApiResponse SyncedData { get; private set; }

        /// <summary>
        /// The message providing details about the sync operation.
        /// </summary>
        public string Message { get; private set; } = string.Empty;


        /// <summary>
        /// The type of sync operation performed (e.g., "Homework", "Assignment").
        /// </summary>
        public string SyncType { get; private set; } = string.Empty;


        /// <summary>
        /// The date and time when the sync was performed.
        /// </summary>
        public DateTime SyncDateTime { get; private set; } = DateTime.UtcNow;


        public enum FailedType
        {
            MonthNotEndError,
            AlreadySynced,

        }

        public static SyncResult Success(string syncType, QuickCampusApiResponse data, string message = "")
        {
            return new SyncResult()
            {
                IsSuccess = true,
                SyncedData = data,
                SyncType = syncType,
                Message = message,
                SyncDateTime = DateTime.UtcNow
            };
        }

        public static SyncResult Failed(string syncType, FailedType failedType, string message = "")
        {
            return new SyncResult()
            {
                IsSuccess = false,
                SyncType = syncType,
                ErrorType = failedType,
                Message = message,
                SyncDateTime = DateTime.UtcNow
            };
        }
    }
}
