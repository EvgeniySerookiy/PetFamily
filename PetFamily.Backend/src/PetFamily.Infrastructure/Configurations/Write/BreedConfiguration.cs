using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.SpesiesManagment.Entities;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Infrastructure.Configurations.Write;

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