// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using PetFamily.Application.Dtos;
//
// namespace PetFamily.Infrastructure.Configurations.Read;
//
// public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
// {
//     public void Configure(EntityTypeBuilder<BreedDto> builder)
//     {
//         builder.ToTable("breeds");
//         
//         builder.HasKey(b => b.Id);
//         
//         builder.Property(b => b.SpeciesId)
//             .HasColumnName("species_id");
//         
//         builder.Property(b => b.BreedName)
//             .HasColumnName("breed_name");
//     }
// }