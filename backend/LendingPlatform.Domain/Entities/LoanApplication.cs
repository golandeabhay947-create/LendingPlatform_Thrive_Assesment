using LendingPlatform.Domain.Enums;

namespace LendingPlatform.Domain.Entities;

public class LoanApplication
{
    public LoanApplication(decimal loanAmount, decimal assetValue, int creditScore)
    {
        LoanAmount = loanAmount;
        AssetValue = assetValue;
        CreditScore = creditScore;
    }

    public decimal LoanAmount { get; }

    public decimal AssetValue { get; }

    public int CreditScore { get; }

    public decimal? LoanToValuePercentage { get; private set; }

    public LoanDecision? Decision { get; private set; }

    internal void RecordDecision(decimal loanToValuePercentage, LoanDecision decision)
    {
        LoanToValuePercentage = loanToValuePercentage;
        Decision = decision;
    }
}
