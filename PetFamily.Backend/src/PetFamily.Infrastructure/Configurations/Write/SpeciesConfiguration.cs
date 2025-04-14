using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.ComplexProperty(s => s.SpeciesName, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(SpeciesName.MAX_SPECIES_NAME_TEXT_LENGTH)
                .HasColumnName("species_name");
        });

        builder.HasMany(s => s.Breeds)
            .WithOne(s => s.Species)
            .HasForeignKey("species_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}