using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
using inventory.Helper;
using Microsoft.AspNetCore.RateLimiting;
namespace inventory.Controllers.AccountController;
public partial class AccountController
{
    [HttpPost("Login")]
    [EnableRateLimiting("IpBasedLimit")]
    public async Task<IActionResult> Login(
            [FromBody] AccountCredentials credentials,
            [FromServices] ILoginAccount loginAccount,
            CancellationToken cancellationToken
            )
    {
        var result = await loginAccount.LoginExecute(credentials, cancellationToken);
        return result.Result();
    }
}