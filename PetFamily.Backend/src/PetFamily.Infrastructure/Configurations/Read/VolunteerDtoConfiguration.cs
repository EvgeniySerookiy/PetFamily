using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.FirstName)
            .HasColumnName("first_name");
        
        builder.Property(p => p.LastName)
            .HasColumnName("last_name");

        builder.Property(p => p.Email);
        builder.Property(p => p.PhoneNumber);
        builder.Property(p => p.IsDeleted);
    }
}
