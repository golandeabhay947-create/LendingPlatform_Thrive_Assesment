using System.ComponentModel.DataAnnotations;

namespace LendingPlatform.Api.Models;

public class LoanApplicationRequest : IValidatableObject
{
    public decimal LoanAmount { get; init; }

    public decimal AssetValue { get; init; }

    public int CreditScore { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (LoanAmount <= 0)
        {
            yield return new ValidationResult(
                "Loan amount must be greater than zero.",
                [nameof(LoanAmount)]);
        }

        if (AssetValue <= 0)
        {
            yield return new ValidationResult(
                "Asset value must be greater than zero.",
                [nameof(AssetValue)]);
        }

        if (CreditScore is < 1 or > 999)
        {
            yield return new ValidationResult(
                "Credit score must be between 1 and 999.",
                [nameof(CreditScore)]);
        }
    }
}
