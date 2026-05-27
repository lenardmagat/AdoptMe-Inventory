using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
using inventory.Extensions;
using System.Xml.XPath;
using inventory.Helper;
namespace inventory.Controllers.AccountController;
public partial class AccountController
{
    [HttpPatch("UpdateProfile")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> UpdateAccount(
        [FromBody] ChangePasswordCredentials credentials,
        [FromServices] IChangePasswordServices changePasswordServices,
        CancellationToken cancellationToken
    )
    {
        int? UserId = User.GetUserId();
        if(UserId == null) return Unauthorized();
        var result = await changePasswordServices.ChangePasswordExecute(credentials, cancellationToken, UserId.Value);
        return result.Result();
    }
}