namespace HomeworkPortal.API.Models
{
    public class FileMetadata : BaseEntity
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public int? AssignmentId { get; set; }
        public int? SubmissionId { get; set; }
    }
}
