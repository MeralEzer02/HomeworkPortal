using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HomeworkPortal.API.Models;

namespace HomeworkPortal.API.Configurations
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.HasOne(s => s.Assignment)
                   .WithMany(a => a.Submissions)
                   .HasForeignKey(s => s.AssignmentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.Student)
                   .WithMany(u => u.Submissions)
                   .HasForeignKey(s => s.StudentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.StudentId);
            builder.HasIndex(x => x.AssignmentId);
        }
    }
}