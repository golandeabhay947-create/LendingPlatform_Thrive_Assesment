using LendingPlatform.Domain.Entities;
using LendingPlatform.Domain.Enums;

namespace LendingPlatform.Domain.Services;

public class LoanDecisionService
{
    private const decimal MinimumLoanAmount = 100_000m;
    private const decimal MillionPounds = 1_000_000m;
    private const decimal MaximumLoanAmount = 1_500_000m;

    public decimal CalculateLoanToValue(LoanApplication application)
    {
        ArgumentNullException.ThrowIfNull(application);
        ValidateAssetValue(application.AssetValue);

        return application.LoanAmount / application.AssetValue * 100m;
    }

    public LoanDecision Evaluate(LoanApplication application)
    {
        ArgumentNullException.ThrowIfNull(application);
        ValidateApplication(application);

        var loanToValue = CalculateLoanToValue(application);
        var decision = DetermineDecision(application.LoanAmount, loanToValue, application.CreditScore);

        application.RecordDecision(loanToValue, decision);
        return decision;
    }

    private static LoanDecision DetermineDecision(decimal loanAmount, decimal loanToValue, int creditScore)
    {
        if (loanAmount < MinimumLoanAmount || loanAmount > MaximumLoanAmount)
        {
            return LoanDecision.Declined;
        }

        if (loanAmount >= MillionPounds)
        {
            return loanToValue <= 60m && creditScore >= 950
                ? LoanDecision.Successful
                : LoanDecision.Declined;
        }

        if (loanToValue < 60m)
        {
            return creditScore >= 750 ? LoanDecision.Successful : LoanDecision.Declined;
        }

        if (loanToValue < 80m)
        {
            return creditScore >= 800 ? LoanDecision.Successful : LoanDecision.Declined;
        }

        if (loanToValue < 90m)
        {
            return creditScore >= 900 ? LoanDecision.Successful : LoanDecision.Declined;
        }

        return LoanDecision.Declined;
    }

    private static void ValidateApplication(LoanApplication application)
    {
        if (application.LoanAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(application.LoanAmount), "Loan amount must be greater than zero.");
        }

        ValidateAssetValue(application.AssetValue);

        if (application.CreditScore is < 1 or > 999)
        {
            throw new ArgumentOutOfRangeException(nameof(application.CreditScore), "Credit score must be between 1 and 999.");
        }
    }

    private static void ValidateAssetValue(decimal assetValue)
    {
        if (assetValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(assetValue), "Asset value must be greater than zero.");
        }
    }
}
