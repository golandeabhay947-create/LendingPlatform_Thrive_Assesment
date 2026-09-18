using LendingPlatform.Domain.Entities;
using LendingPlatform.Domain.Enums;
using LendingPlatform.Domain.Services;

namespace LendingPlatform.Tests;

public class LoanDecisionServiceTests
{
    private readonly LoanDecisionService _service = new();

    [Fact]
    public void CalculateLoanToValue_ReturnsPercentage()
    {
        var application = new LoanApplication(250_000m, 500_000m, 750);

        var loanToValue = _service.CalculateLoanToValue(application);

        Assert.Equal(50m, loanToValue);
    }

    [Fact]
    public void Evaluate_RecordsCalculatedLtvAndDecisionOnApplication()
    {
        var application = new LoanApplication(250_000m, 500_000m, 750);

        var decision = _service.Evaluate(application);

        Assert.Equal(LoanDecision.Successful, decision);
        Assert.Equal(50m, application.LoanToValuePercentage);
        Assert.Equal(LoanDecision.Successful, application.Decision);
    }

    [Theory]
    [MemberData(nameof(LoanAmountCases))]
    public void Evaluate_AppliesLoanAmountBoundaries(decimal loanAmount, LoanDecision expectedDecision)
    {
        var application = new LoanApplication(loanAmount, 3_000_000m, 950);

        var decision = _service.Evaluate(application);

        Assert.Equal(expectedDecision, decision);
    }

    [Theory]
    [MemberData(nameof(UnderMillionLtvCases))]
    public void Evaluate_AppliesUnderMillionLtvBoundaries(
        decimal loanAmount,
        decimal assetValue,
        int creditScore,
        LoanDecision expectedDecision)
    {
        var application = new LoanApplication(loanAmount, assetValue, creditScore);

        var decision = _service.Evaluate(application);

        Assert.Equal(expectedDecision, decision);
    }

    [Theory]
    [MemberData(nameof(MillionOrMoreCases))]
    public void Evaluate_AppliesMillionOrMoreRules(
        decimal loanAmount,
        decimal assetValue,
        int creditScore,
        LoanDecision expectedDecision)
    {
        var application = new LoanApplication(loanAmount, assetValue, creditScore);

        var decision = _service.Evaluate(application);

        Assert.Equal(expectedDecision, decision);
    }

    [Theory]
    [InlineData(100_000, 0, 750)]
    [InlineData(100_000, -1, 750)]
    public void Evaluate_ThrowsForNonPositiveAssetValue(decimal loanAmount, decimal assetValue, int creditScore)
    {
        var application = new LoanApplication(loanAmount, assetValue, creditScore);

        Assert.Throws<ArgumentOutOfRangeException>(() => _service.Evaluate(application));
    }

    [Theory]
    [InlineData(100_000, 1_000_000, 0)]
    [InlineData(100_000, 1_000_000, 1000)]
    public void Evaluate_ThrowsForCreditScoreOutsideAllowedRange(decimal loanAmount, decimal assetValue, int creditScore)
    {
        var application = new LoanApplication(loanAmount, assetValue, creditScore);

        Assert.Throws<ArgumentOutOfRangeException>(() => _service.Evaluate(application));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(999)]
    public void Evaluate_AcceptsCreditScoreAtAllowedRangeBoundaries(int creditScore)
    {
        var application = new LoanApplication(100_000m, 2_000_000m, creditScore);

        var exception = Record.Exception(() => _service.Evaluate(application));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Evaluate_ThrowsForNonPositiveLoanAmount(decimal loanAmount)
    {
        var application = new LoanApplication(loanAmount, 1_000_000m, 750);

        Assert.Throws<ArgumentOutOfRangeException>(() => _service.Evaluate(application));
    }

    public static IEnumerable<object[]> LoanAmountCases =>
    [
        [99_999m, LoanDecision.Declined],
        [100_000m, LoanDecision.Successful],
        [1_000_000m, LoanDecision.Successful],
        [1_500_000m, LoanDecision.Successful],
        [1_500_001m, LoanDecision.Declined]
    ];

    public static IEnumerable<object[]> UnderMillionLtvCases =>
    [
        [599_999m, 1_000_000m, 750, LoanDecision.Successful],
        [599_999m, 1_000_000m, 749, LoanDecision.Declined],
        [600_000m, 1_000_000m, 800, LoanDecision.Successful],
        [600_001m, 1_000_000m, 799, LoanDecision.Declined],
        [799_999m, 1_000_000m, 800, LoanDecision.Successful],
        [799_999m, 1_000_000m, 799, LoanDecision.Declined],
        [800_000m, 1_000_000m, 900, LoanDecision.Successful],
        [800_001m, 1_000_000m, 899, LoanDecision.Declined],
        [899_999m, 1_000_000m, 900, LoanDecision.Successful],
        [899_999m, 1_000_000m, 899, LoanDecision.Declined],
        [900_000m, 1_000_000m, 999, LoanDecision.Declined],
        [900_001m, 1_000_000m, 999, LoanDecision.Declined]
    ];

    public static IEnumerable<object[]> MillionOrMoreCases =>
    [
        [1_200_000m, 2_000_000m, 950, LoanDecision.Successful],
        [1_200_000m, 2_000_000m, 949, LoanDecision.Declined],
        [1_200_001m, 2_000_000m, 950, LoanDecision.Declined]
    ];
}
