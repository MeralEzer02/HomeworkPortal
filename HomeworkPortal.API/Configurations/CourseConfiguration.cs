using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HomeworkPortal.API.Models;

namespace HomeworkPortal.API.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.HasIndex(x => x.TeacherId);
        }
    }
}