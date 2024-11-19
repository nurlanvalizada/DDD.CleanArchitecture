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

        var addressOwned = builder.OwnsOne(o => o.Address);
        
        addressOwned.Property(x => x.Country).HasMaxLength(100);
        addressOwned.Property(x => x.City).HasMaxLength(100);
        addressOwned.Property(x => x.State).HasMaxLength(100);
        addressOwned.Property(x => x.Street).HasMaxLength(200);
        addressOwned.Property(x => x.ZipCode).HasMaxLength(20);
    }
}