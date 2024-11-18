using AppDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TodoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.ToTable("Tasks");

        builder.HasOne(t => t.AssignedPerson).WithMany(p => p.Tasks)
               .HasForeignKey(t => t.AssignedPersonId);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(100);
    }
}