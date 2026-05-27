using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
namespace inventory.Controllers.AccountController;
public partial class AccountController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] AccountCredentials AccountData, 
        [FromServices] IRegisterService registerService,
        CancellationToken cancellationToken
        )
    {
        var result = await registerService.ExecuteRegister(AccountData, cancellationToken);
        if(!result.IsSuccess)
            return StatusCode(result.StatusCode, new
            {
                error = result.Error,
                timestamp = DateTime.UtcNow
            }
            );
        return Ok(result);
    }
}