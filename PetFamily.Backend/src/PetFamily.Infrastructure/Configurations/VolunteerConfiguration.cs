using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        
        builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("pets_id");

        builder.ComplexProperty(v => v.FullName, fb =>
        {
                fb.Property(f => f.FirstName)
                        .IsRequired()
                        .HasMaxLength(FullName.MAX_ALL_NAME_TEXT_LENGTH)
                        .HasColumnName("first_name");
                
                fb.Property(f => f.LastName)
                        .IsRequired()
                        .HasMaxLength(FullName.MAX_ALL_NAME_TEXT_LENGTH)
                        .HasColumnName("last_name");
                
                fb.Property(f => f.MiddleName)
                        .HasMaxLength(FullName.MAX_ALL_NAME_TEXT_LENGTH)
                        .HasColumnName("middle_name");
        });
        
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
        
        builder.OwnsOne(p => p.TransferSocialNetworkList, tb =>
        {
                tb.ToJson();
                
                tb.OwnsMany(t => t.SocialNetworks, sb =>
                {
                        sb.Property(s => s.NetworkName)
                                .IsRequired()
                                .HasMaxLength(SocialNetwork.MAX_NETWORK_NAME_TEXT_LENGTH)
                                .HasColumnName("network_name");
                        
                        sb.Property(s => s.NetworkAddress)
                                .IsRequired()
                                .HasMaxLength(SocialNetwork.MAX_NETWORK_ADDRESS_TEXT_LENGTH)
                                .HasColumnName("network_address");
                });
        });
        
        builder.OwnsOne(p => p.TransferRequisitesForHelpsList, tb =>
        {
                tb.ToJson();
                
                tb.OwnsMany(t => t.RequisitesForHelps, rb =>
                {
                        rb.Property(r => r.Recipient)
                                .IsRequired()
                                .HasMaxLength(RequisitesForHelp.MAX_RECIPIENT_TEXT_LENGTH)
                                .HasColumnName("recipient");
                        
                        rb.Property(r => r.PaymentDetails)
                                .IsRequired()
                                .HasMaxLength(RequisitesForHelp.MAX_PAYMENT_DETAILS_TEXT_LENGTH)
                                .HasColumnName("payment_details");
                });
        });
    }
}