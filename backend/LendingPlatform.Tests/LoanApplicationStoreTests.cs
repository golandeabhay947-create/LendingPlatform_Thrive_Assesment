using LendingPlatform.Api.Models;
using LendingPlatform.Api.Services;
using LendingPlatform.Domain.Enums;
using LendingPlatform.Domain.Services;

namespace LendingPlatform.Tests;

public class LoanApplicationStoreTests
{
    [Fact]
    public void Submit_ReturnsSuccessfulDecisionAndCalculatedLtv()
    {
        var store = CreateStore();

        var response = store.Submit(new LoanApplicationRequest
        {
            LoanAmount = 500_000m,
            AssetValue = 1_000_000m,
            CreditScore = 800
        });

        Assert.Equal(LoanDecision.Successful, response.Decision);
        Assert.Equal(50m, response.Ltv);
    }

    [Fact]
    public void Submit_ReturnsDeclinedDecision()
    {
        var store = CreateStore();

        var response = store.Submit(new LoanApplicationRequest
        {
            LoanAmount = 500_000m,
            AssetValue = 1_000_000m,
            CreditScore = 749
        });

        Assert.Equal(LoanDecision.Declined, response.Decision);
        Assert.Equal(50m, response.Ltv);
    }

    [Theory]
    [InlineData(0, 1_000_000, 800)]
    [InlineData(500_000, 0, 800)]
    [InlineData(500_000, 1_000_000, 0)]
    [InlineData(500_000, 1_000_000, 1000)]
    public void Submit_ThrowsForInvalidTechnicalInput(decimal loanAmount, decimal assetValue, int creditScore)
    {
        var store = CreateStore();
        var request = new LoanApplicationRequest
        {
            LoanAmount = loanAmount,
            AssetValue = assetValue,
            CreditScore = creditScore
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => store.Submit(request));
    }

    [Fact]
    public void GetStatistics_ReturnsZeroValuesWhenNoApplicationsExist()
    {
        var statistics = CreateStore().GetStatistics();

        Assert.Equal(0, statistics.SuccessfulApplications);
        Assert.Equal(0, statistics.DeclinedApplications);
        Assert.Equal(0m, statistics.TotalLoanValue);
        Assert.Equal(0m, statistics.MeanLtv);
    }

    [Fact]
    public void GetStatistics_CountsSuccessfulAndDeclinedApplications()
    {
        var store = CreateStore();
        SubmitSuccessfulApplication(store);
        SubmitDeclinedApplication(store);

        var statistics = store.GetStatistics();

        Assert.Equal(1, statistics.SuccessfulApplications);
        Assert.Equal(1, statistics.DeclinedApplications);
    }

    [Fact]
    public void GetStatistics_IncludesOnlySuccessfulLoansInTotalLoanValue()
    {
        var store = CreateStore();
        SubmitSuccessfulApplication(store);
        SubmitDeclinedApplication(store);

        var statistics = store.GetStatistics();

        Assert.Equal(500_000m, statistics.TotalLoanValue);
    }

    [Fact]
    public void GetStatistics_IncludesDeclinedApplicationsInMeanLtv()
    {
        var store = CreateStore();
        SubmitSuccessfulApplication(store);
        SubmitDeclinedApplication(store);

        var statistics = store.GetStatistics();

        Assert.Equal(65m, statistics.MeanLtv);
    }

    private static LoanApplicationStore CreateStore()
    {
        return new LoanApplicationStore(new LoanDecisionService());
    }

    private static void SubmitSuccessfulApplication(LoanApplicationStore store)
    {
        store.Submit(new LoanApplicationRequest
        {
            LoanAmount = 500_000m,
            AssetValue = 1_000_000m,
            CreditScore = 800
        });
    }

    private static void SubmitDeclinedApplication(LoanApplicationStore store)
    {
        store.Submit(new LoanApplicationRequest
        {
            LoanAmount = 800_000m,
            AssetValue = 1_000_000m,
            CreditScore = 899
        });
    }
}
