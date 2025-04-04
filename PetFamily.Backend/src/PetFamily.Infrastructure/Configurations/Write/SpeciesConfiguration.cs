using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.SpesiesManagment.Entities;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

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

        builder.ComplexProperty(s => s.PetName, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PetName.MAX_PET_NAME_TEXT_LENGTH)
                .HasColumnName("pet_name");
        });

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}