using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
using inventory.Helper;
using Microsoft.AspNetCore.RateLimiting;
namespace inventory.Controllers.AccountController;
public partial class AccountController
{
    [HttpPost("register")]
    [EnableRateLimiting("IpBasedLimit")]
    public async Task<IActionResult> Register(
        [FromBody] AccountCredentials AccountData, 
        [FromServices] IRegisterService registerService,
        CancellationToken cancellationToken
        )
    {
        var result = await registerService.ExecuteRegister(AccountData, cancellationToken);
        return result.Result();
    }
}