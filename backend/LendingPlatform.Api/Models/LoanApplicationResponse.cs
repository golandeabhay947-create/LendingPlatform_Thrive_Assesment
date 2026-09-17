using LendingPlatform.Domain.Enums;

namespace LendingPlatform.Api.Models;

public record LoanApplicationResponse(LoanDecision Decision, decimal Ltv);
