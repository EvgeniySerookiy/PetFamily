using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Infrastructure.Configurations.Write;

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
        
        builder.Property(p => p.VolunteerId)
            .HasConversion(
                v => v.Value,
                value => VolunteerId.Create(value));
        
        builder.ComplexProperty(p => p.PetName, nb =>
        {
            nb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PetName.MAX_PET_NAME_TEXT_LENGTH)
                .HasColumnName("pet_name");
        });
        
        builder.ComplexProperty(p => p.SpeciesId, sb =>
        {
            sb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("species_id");
        });
        
        builder.ComplexProperty(p => p.BreedId, bb =>
        {
            bb.Property(b => b.Value)
                .IsRequired()
                .HasColumnName("breed_id");
        });
        
        builder.OwnsOne(p => p.PetPhotos, pb =>
        {
            pb.ToJson("photos");
                
            pb.OwnsMany(p => p.Values, vb =>
            {
                vb.Property(v => v.PathToStorage)
                    .HasConversion(
                        v => v.Path,
                        value => PhotoPath.Create(value).Value)
                    .IsRequired();
            });
        });

        builder.ComplexProperty(p => p.Title, tb =>
        {
            tb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Title.MAX_TITLE_TEXT_LENGTH)
                .HasColumnName("title");
        });
        
        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Description.MAX_DESCRIPTION_TEXT_LENGTH)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(p => p.Position, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasColumnName("serial_number");
        });
        
        builder.ComplexProperty(p => p.Color, cb =>
        {
            cb.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(Color.MAX_COLOR_TEXT_LENGTH)
                .HasColumnName("color");
        });
        
        builder.ComplexProperty(p => p.PetHealthInformation, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PetHealthInformation.MAX_HEALTH_INFORMATION_TEXT_LENGTH)
                .HasColumnName("pet_health_information");
        });

        builder.ComplexProperty(p => p.PetAddress, pb =>
        {
            pb.Property(p => p.Region)
                .IsRequired()
                .HasMaxLength(Address.MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
                .HasColumnName("region");
            
            pb.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(Address.MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
                .HasColumnName("city");
            
            pb.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(Address.MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
                .HasColumnName("street");
            
            pb.Property(p => p.Building)
                .IsRequired()
                .HasMaxLength(Address.MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
                .HasColumnName("building");
            
            pb.Property(p => p.Apartment)
                .HasMaxLength(Address.MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
                .HasColumnName("apartment");
        });
        
        builder.ComplexProperty(p => p.OwnerPhoneNumber, ob =>
        {
            ob.Property(o => o.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_PHONE_NUMBER_TEXT_LENGTH)
                .HasColumnName("owner_phone_number");
        });
        
        builder.ComplexProperty(p => p.Size, sb =>
        {
            sb.Property(s => s.Weight)
                .IsRequired()
                .HasColumnName("weight");
    
            sb.Property(s => s.Height)
                .IsRequired()
                .HasColumnName("height");
        });
        
        builder.ComplexProperty(p => p.IsNeutered, ib =>
        {
            ib.Property(i => i.Value)
                .IsRequired()
                .HasColumnName("is_neutered");
        });
        
        builder.ComplexProperty(p => p.IsVaccinated, ib =>
        {
            ib.Property(i => i.Value)
                .IsRequired()
                .HasColumnName("is_vaccinated");
        });
        
        builder.Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth");
        
        builder.Property(p => p.Status)
            .HasConversion(
                s => s.ToString(),
                s => (AssistanceStatus)Enum.Parse(typeof(AssistanceStatus), s))
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_STATUS_TEXT_LENGTH)
            .HasColumnName("status");
        
        builder.Property(p => p.DateOfCreation)
            .IsRequired()
            .HasColumnName("date_of_creation");
        
        builder.Property(p => p.IsDeleted)
            .HasColumnName("is_deleted");
    }
}