using LendingPlatform.Api.Models;
using LendingPlatform.Domain.Entities;
using LendingPlatform.Domain.Enums;
using LendingPlatform.Domain.Services;

namespace LendingPlatform.Api.Services;

public class LoanApplicationStore
{
    private readonly List<LoanApplication> _applications = [];
    private readonly object _lock = new();
    private readonly LoanDecisionService _loanDecisionService;

    public LoanApplicationStore(LoanDecisionService loanDecisionService)
    {
        _loanDecisionService = loanDecisionService;
    }

    public LoanApplicationResponse Submit(LoanApplicationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = new LoanApplication(request.LoanAmount, request.AssetValue, request.CreditScore);
        var decision = _loanDecisionService.Evaluate(application);

        lock (_lock)
        {
            _applications.Add(application);
        }

        return new LoanApplicationResponse(decision, application.LoanToValuePercentage!.Value);
    }

    public LoanStatisticsResponse GetStatistics()
    {
        lock (_lock)
        {
            var successfulApplications = _applications.Count(application => application.Decision == LoanDecision.Successful);
            var declinedApplications = _applications.Count(application => application.Decision == LoanDecision.Declined);
            var totalLoanValue = _applications
                .Where(application => application.Decision == LoanDecision.Successful)
                .Sum(application => application.LoanAmount);
            var meanLtv = _applications.Count == 0
                ? 0m
                : _applications.Average(application => application.LoanToValuePercentage!.Value);

            return new LoanStatisticsResponse(
                successfulApplications,
                declinedApplications,
                totalLoanValue,
                meanLtv);
        }
    }
}
