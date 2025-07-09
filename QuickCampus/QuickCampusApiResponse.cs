namespace My.QuickCampus.QuickCampus
{
    public class QuickCampusApiResponse
    {
        public string Message { get; set; }

        public int TotalRecords { get; set; }

        public List<QuickCampusApiResponseItem> Data { get; set; }
    }

    public class QuickCampusApiResponseItem
    {
        public string uid { get; set; }
        public string Title { get; set; }
        public string ClassSectionName { get; set; }
        public SubjectResponse SubjectResponse { get; set; }
        public string SubjectName { get; set; }
        public string PostedDate { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public List<string> Documents { get; set; }
        public List<string> Images { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EditedDate { get; set; }
        public string CreatedByEmployeeNo { get; set; }
        public string CreatedByEmployeeName { get; set; }
        public string EditedByEmployeeNo { get; set; }
        public string EditedByEmployeeName { get; set; }
    }

    public class SubjectResponse
    {
        public string SubjectName { get; set; }

        public string CreatedUser { get; set; }

        public string EditedUser { get; set; }

        public string Createddate { get; set; }

        public string Editeddate { get; set; }
    }
}
