using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
namespace inventory.Controllers.AccountController;
public partial class AccountController
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login(
            [FromBody] AccountCredentials credentials,
            [FromServices] ILoginAccount loginAccount,
            CancellationToken cancellationToken
            )
    {
        var result = await loginAccount.LoginExecute(credentials, cancellationToken);
        if(!result.IsSuccess)
            return StatusCode(
                result.StatusCode, new
                {
                    error = result.Error,
                    timestamp = DateTime.UtcNow
                }
            );
        return Ok(result);
    }
}