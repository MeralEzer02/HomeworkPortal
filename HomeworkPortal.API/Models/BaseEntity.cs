using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.API.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
