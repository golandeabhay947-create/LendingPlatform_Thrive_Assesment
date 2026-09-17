using LendingPlatform.Api.Models;
using LendingPlatform.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LendingPlatform.Api.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    private readonly LoanApplicationStore _loanApplicationStore;

    public LoansController(LoanApplicationStore loanApplicationStore)
    {
        _loanApplicationStore = loanApplicationStore;
    }

    [HttpPost("applications")]
    public ActionResult<LoanApplicationResponse> SubmitApplication(LoanApplicationRequest request)
    {
        try
        {
            return Ok(_loanApplicationStore.Submit(request));
        }
        catch (ArgumentOutOfRangeException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpGet("statistics")]
    public ActionResult<LoanStatisticsResponse> GetStatistics()
    {
        return Ok(_loanApplicationStore.GetStatistics());
    }
}
