using AppDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        builder.Property(t => t.Name)
               .HasMaxLength(20)
               .IsRequired();

        builder.OwnsOne(o => o.Address);
    }
}