namespace My.QuickCampus.QuickCampus
{
    public static class QuickCampusApiResponseExtension
    {
        private static Dictionary<string, string> _subjectMaping = new Dictionary<string, string>
        {
            { "english", "eng" },
            { "mathematics", "maths" },
            { "hindi", "hin" },
            { "evs", "evs" }
        };

        private static Dictionary<string, string> _typeMaping = new Dictionary<string, string>
        {
            { "homework", "hw" },
            { "assignment", "asgmt" },
        };

        private static Dictionary<string, string> _classMaping = new Dictionary<string, string>
        {
            { "iii", "iii" },
            { "iii-c", "iii" },
            { "iii-d", "iii" },
            { "v-a", "v" },
            { "v", "v" },
        };

        public static string GetDownloadInfo(this QuickCampusApiResponseItem dateItem, int serial, string fileName, int fileCount)
        {
            var date = dateItem.PostedDate.ToString("yyyy_MM_dd");

            var isGetClass = _classMaping.TryGetValue(dateItem.ClassSectionName.ToLowerInvariant(), out var studentClass);
            if (!isGetClass)
            {
                studentClass = dateItem.ClassSectionName.ToLowerInvariant();
            }


            var isGetType = _typeMaping.TryGetValue(dateItem.Type.ToLowerInvariant(), out var type);
            if (!isGetType)
            {
                type = dateItem.Type.ToLowerInvariant();
            }

            var isGetSub = _subjectMaping.TryGetValue(dateItem.SubjectName.ToLowerInvariant(), out var subject);
            if (!isGetSub)
            {
                subject = dateItem.SubjectName.ToLowerInvariant();
            }

            // WARNING: Dont change this file format specially where ___ is because actual file name is extracted by spliting with ___.
            return $"{studentClass}__{type}__{serial}__{date}__{subject}_{fileCount}___{fileName}";
        }

    }
}
