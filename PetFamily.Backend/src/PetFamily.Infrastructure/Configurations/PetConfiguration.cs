using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain;
using PetFamily.Domain.PetContext;
using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.Shared;
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
        
        builder.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                .HasColumnName("name");
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
        
        builder.ComplexProperty(p => p.Color, cb =>
        {
            cb.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
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
            pb.ComplexProperty(p => p.Region, rb =>
            {
                rb.Property(r => r.Value)
                    .IsRequired()
                    .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                    .HasColumnName("region");
            });
            
            pb.ComplexProperty(p => p.City, cb =>
            {
                cb.Property(c => c.Value)
                    .IsRequired()
                    .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                    .HasColumnName("city");
            });
            
            pb.ComplexProperty(p => p.Street, sb =>
            {
                sb.Property(s => s.Value)
                    .IsRequired()
                    .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                    .HasColumnName("street");
            });
            
            pb.ComplexProperty(p => p.Building, bb =>
            {
                bb.Property(b => b.Value)
                    .IsRequired()
                    .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                    .HasColumnName("building");
            });
                
            pb.Property(p => p.Apartment)
                .HasMaxLength(Address.MAX_APARTMENT_TEXT_LENGTH)
                .HasColumnName("apartment");
            
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
                .HasMaxLength(Constants.MAX_STATUS_TEXT_LENGTH)
                .HasColumnName("status");
            
            builder.OwnsOne(p => p.TransferRequisitesForHelpsList, tb =>
            {
                tb.ToJson();

                tb.OwnsMany(t => t.RequisitesForHelps, rb =>
                {
                    rb.OwnsOne(r => r.Title, tb =>
                    {
                        tb.Property(t => t.Value)
                            .IsRequired()
                            .HasMaxLength(Title.MAX_TITLE_TEXT_LENGTH)
                            .HasColumnName("title");
                    });
                        
                    rb.OwnsOne(r => r.Description, db =>
                    {
                        db.Property(t => t.Value)
                            .IsRequired()
                            .HasMaxLength(Description.MAX_DESCRIPTION_TEXT_LENGTH)
                            .HasColumnName("description");
                    });
                });
            });
            
            builder.Property(p => p.DateOfCreation)
                .IsRequired()
                .HasColumnName("date_of_creation");
        });
        
    }
}