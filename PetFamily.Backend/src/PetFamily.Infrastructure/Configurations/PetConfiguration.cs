using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetContext;
using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("Title");
        
    }
}