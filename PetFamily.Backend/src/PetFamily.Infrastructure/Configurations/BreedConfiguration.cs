using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.SpeciesContext.SpeciesEntities;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("Breeds");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));

        builder.ComplexProperty(b => b.PetName, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PetName.MAX_PET_NAME_TEXT_LENGTH)
                .HasColumnName("pet_name");
        });
    }
}