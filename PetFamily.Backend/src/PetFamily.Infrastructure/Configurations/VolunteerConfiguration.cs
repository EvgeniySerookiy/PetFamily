using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("Volunteers");
        
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, fb =>
        {
            fb.ComplexProperty(f => f.FirstName, fb =>
            {
                fb.Property(f => f.Value)
                    .IsRequired()
                    .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                    .HasColumnName("first_name");
            });
            
            fb.ComplexProperty(f => f.LastName, lb =>
            {
                lb.Property(l => l.Value)
                    .IsRequired()
                    .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                    .HasColumnName("last_name");
            });
            
            fb.Property(f => f.MiddleName)
                .HasMaxLength(FullName.MAX_MIDDLE_NAME_TEXT_LENGTH)
                .HasColumnName("middle_name");
        });
        
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("pets_id");

        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(Email.MAX_EMAIL_TEXT_LENGTH)
                .HasColumnName("email");
        });
        
        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Description.MAX_DESCRIPTION_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.ComplexProperty(v => v.YearsOfExperience, yb =>
        {
            yb.Property(y => y.Value)
                .IsRequired()
                .HasColumnName("years_of_experience");
        });
        
        builder.Property(v => v.PetsRehomed)
            .HasColumnName("pets_rehomed");
        
        builder.Property(v => v.PetsSeekingHome)
            .HasColumnName("pets_seeking_home");
        
        builder.Property(v => v.PetsUnderTreatment)
            .HasColumnName("pets_under_treatment");

        builder.ComplexProperty(v => v.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_PHONE_NUMBER_TEXT_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.ComplexProperty(v => v.AssistanceRequisites, ab =>
        {
            ab.ComplexProperty(a => a.Description, db =>
            {
                db.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Description.MAX_DESCRIPTION_TEXT_LENGTH)
                    .HasColumnName("description");
            });
            
            ab.ComplexProperty(a => a.Title, tb =>
            {
                tb.Property(t => t.Value)
                    .IsRequired()
                    .HasMaxLength(Title.MAX_TITLE_TEXT_LENGTH)
                    .HasColumnName("title");
            });
        });
        
        builder.OwnsOne(p => p.TransferSocialNetworkList, tb =>
        {
            tb.ToJson();

            tb.OwnsMany(t => t.SocialNetworks, sb =>
            {
                sb.OwnsOne(s => s.NetworkName, nb =>
                {
                    nb.Property(t => t.Value)
                        .IsRequired()
                        .HasMaxLength(NotEmptyString.MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
                        .HasColumnName("network_name");
                });
                        
                sb.OwnsOne(r => r.NetworkAddress, nb =>
                {
                    nb.Property(t => t.Value)
                        .IsRequired()
                        .HasMaxLength(Description.MAX_DESCRIPTION_TEXT_LENGTH)
                        .HasColumnName("network_address");
                });
            });
        });
    }
}