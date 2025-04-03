using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("Volunteers");
        
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.Pets)
            .WithOne()
            .HasForeignKey(p => p.VolunteerId);
    }
}