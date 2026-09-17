namespace LendingPlatform.Api.Models;

public record LoanStatisticsResponse(
    int SuccessfulApplications,
    int DeclinedApplications,
    decimal TotalLoanValue,
    decimal MeanLtv);
