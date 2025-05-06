// using System.Text.Json;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using PetFamily.Application.Dtos;
//
// namespace PetFamily.Infrastructure.Configurations.Read;
//
// public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
// {
//     public void Configure(EntityTypeBuilder<PetDto> builder)
//     {
//         builder.ToTable("pets");
//         builder.HasKey(p => p.Id);
//         builder.Property(p => p.SpeciesId);
//         builder.Property(p => p.BreedId);
//         builder.Property(p => p.PetName);
//         builder.Property(p => p.DateOfBird);
//         builder.Property(p => p.Region);
//         builder.Property(p => p.City);
//         builder.Property(p => p.Color);
//         builder.Property(p => p.Position);
//         builder.Property(p => p.IsDeleted);
//         builder.Property(p => p.PetPhotos)
//             .HasConversion(
//                 files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
//                 json => JsonSerializer.Deserialize<PetPhotoDto[]>(json, JsonSerializerOptions.Default)!);
//     }
// }