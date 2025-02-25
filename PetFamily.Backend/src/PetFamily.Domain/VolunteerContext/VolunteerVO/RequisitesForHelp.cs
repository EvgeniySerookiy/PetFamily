using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record RequisitesForHelp
{
    public const int MAX_RECIPIENT_TEXT_LENGTH = 100;
    public const int MAX_PAYMENT_DETAILS_TEXT_LENGTH = 200;
    public string Recipient { get; }
    public string PaymentDetails { get; }

    private RequisitesForHelp(string recipient, string paymentDetails)
    {
        Recipient = recipient;
        PaymentDetails = paymentDetails;
    }

    public static Result<RequisitesForHelp, Error> Create(string recipient , string paymentDetails)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            return Errors.General.ValueIsRequired("Recipient");
        
        if (string.IsNullOrWhiteSpace(paymentDetails))
            return Errors.General.ValueIsRequired("Payment details");
        
        if (recipient.Length > MAX_RECIPIENT_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Recipient", MAX_RECIPIENT_TEXT_LENGTH);
        
        if (paymentDetails.Length > MAX_PAYMENT_DETAILS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Payment details", MAX_PAYMENT_DETAILS_TEXT_LENGTH);
        
        return new RequisitesForHelp(recipient, paymentDetails);
    }
}