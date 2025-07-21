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
            { "homework", "hm" },
            { "assignment", "asgmt" },
        };

        private static Dictionary<string, string> _classMaping = new Dictionary<string, string>
        {
            { "iii", "III" },
            { "iii-c", "III" },
            { "iii-d", "III" },
            { "v-a", "V" },
            { "v", "V" },
        };

        public static string GetDownloadInfo(this QuickCampusApiResponseItem dateItem, string fileName, int fileCount)
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

            return $"{date}__{studentClass}__{type}__{subject}_{fileCount}___{fileName}";
        }

    }
}
