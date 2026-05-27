using inventory.DTOs;
using inventory.Extensions;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
using inventory.Helper;
using Microsoft.AspNetCore.Mvc;

namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpDelete("DeleteItem")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> DeleteItem(
        [FromBody] DeleteItem itemDetails,
        [FromServices] IDeleteItemService deleteItemService,
        CancellationToken cancellation
    )
    {
        var UserId = User.GetUserId();
        if(!UserId.HasValue) return Unauthorized();
        var result = await deleteItemService.ExecuteDeleteItem(UserId.Value, itemDetails, cancellation);
        return result.Result();
    }
}