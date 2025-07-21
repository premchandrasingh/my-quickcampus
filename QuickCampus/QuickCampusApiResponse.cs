using My.QuickCampus.Entities;

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

        public DateOnly PostedDate { get; set; }

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


        public Homework ConvertToHomework(long gradeId)
        {
            var rec = new Homework
            {
                QuickCampusId = uid,
                GradeId = gradeId,
                Title = Title,
                ClassSectionName = ClassSectionName,
                SubjectName = SubjectName,
                PostedDate = PostedDate,
                Body = Body,
                Type = Type,
                QuickCampusCreatedBy = CreatedBy,
                QuickCampusEditedBy = EditedBy,
                QuickCampusCreatedDate = CreatedDate,
                QuickCampusEditedDate = EditedDate,
                CreatedByEmployeeNo = CreatedByEmployeeNo,
                CreatedByEmployeeName = CreatedByEmployeeName,
                EditedByEmployeeNo = EditedByEmployeeNo,
                EditedByEmployeeName = EditedByEmployeeName,
            };

            if (Documents != null && Documents.Count > 0)
            {
                var docs = Documents.Select((doc, idx) => new MediaFile
                {
                    Homework = rec,
                    HomeworkId = rec.HomeworkId,
                    MediaType = "Document",
                    FileExtension = Path.GetExtension(doc),
                    QuickCampusFileName = doc,
                    FileName = $"{rec.PostedDate.ToString("yyyy_MM_dd")}__{rec.Type}__{rec.SubjectName}__{idx + 1}_{doc}",
                }).ToList();
                rec.MediaFiles.AddRange(docs);
            }

            if (Images != null && Images.Count > 0)
            {
                var imgs = Images.Select((doc, idx) => new MediaFile
                {
                    Homework = rec,
                    HomeworkId = rec.HomeworkId,
                    MediaType = "Image",
                    FileExtension = Path.GetExtension(doc),
                    QuickCampusFileName = doc,
                    FileName = $"{rec.PostedDate.ToString("yyyy_MM_dd")}__{rec.Type}__{rec.SubjectName}__{idx + 1}_{doc}",
                }).ToList();
                rec.MediaFiles.AddRange(imgs);
            }

            return rec;
        }

        public Assignment ConvertToAssignment(long gradeId)
        {
            var rec = new Assignment
            {
                QuickCampusId = uid,
                GradeId = gradeId,
                Title = Title,
                ClassSectionName = ClassSectionName,
                SubjectName = SubjectName,
                PostedDate = PostedDate,
                Body = Body,
                Type = Type,
                QuickCampusCreatedBy = CreatedBy,
                QuickCampusEditedBy = EditedBy,
                QuickCampusCreatedDate = CreatedDate,
                QuickCampusEditedDate = EditedDate,
                CreatedByEmployeeNo = CreatedByEmployeeNo,
                CreatedByEmployeeName = CreatedByEmployeeName,
                EditedByEmployeeNo = EditedByEmployeeNo,
                EditedByEmployeeName = EditedByEmployeeName,
            };

            if (Documents != null && Documents.Count > 0)
            {
                rec.MediaFiles.AddRange(Documents.Select((doc, idx) => new MediaFile
                {
                    MediaType = "Document",
                    FileExtension = Path.GetExtension(doc),
                    QuickCampusFileName = doc,
                    FileName = $"{rec.PostedDate.ToString("yyyy_MM_dd")}__{rec.Type}__{rec.SubjectName}__{idx + 1}_{doc}",
                }).ToList());
            }

            if (Images != null && Images.Count > 0)
            {
                rec.MediaFiles.AddRange(Images.Select((doc, idx) => new MediaFile
                {
                    MediaType = "Image",
                    FileExtension = Path.GetExtension(doc),
                    QuickCampusFileName = doc,
                    FileName = $"{rec.PostedDate.ToString("yyyy_MM_dd")}__{rec.Type}__{rec.SubjectName}__{idx + 1}_{doc}",
                }).ToList());
            }

            return rec;
        }
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
