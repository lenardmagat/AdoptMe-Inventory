using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
namespace inventory.Controllers.AdminControllers;
public partial class AdminController
{
    [HttpPost("Upsert")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upsert(
        [FromBody] StorageItems items,
        [FromServices] IUpsertService addUpdateService,
        CancellationToken cancellationToken
    )
    {
        var result = await addUpdateService.UpsertExecute(items, cancellationToken);
        return Ok(result);
    }
}