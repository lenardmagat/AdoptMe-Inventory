using Microsoft.AspNetCore.Mvc;
using inventory.Services;
using inventory.DTOs;
using Microsoft.AspNetCore.Authorization;
using inventory.Extensions;
using inventory.Helper;
namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpPut("UpsertItem")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> UpsertItem(
        [FromBody] UpsertItemDetails itemDetails,
        [FromServices] IUpsertItemasync upsertItemasync,
        CancellationToken cancellation
    )
    {
        var UserId = User.GetUserId();
        if (!UserId.HasValue) return Unauthorized();
        var result = await upsertItemasync.ExecuteUpsertItemasync(UserId.Value, itemDetails, cancellation);
        return result.Result();
    }
}