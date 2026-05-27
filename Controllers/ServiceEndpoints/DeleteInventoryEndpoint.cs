using inventory.DTOs;
using inventory.Extensions;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using inventory.Helper;
namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpDelete("DeleteInventory/{InventoryId}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> DeleteInventory(
        DeleteInventory InventoryId,
        [FromServices] IDeleteInvetoryService deleteInvetory,
        CancellationToken cancellation)
    {
        var UserId = User.GetUserId();
        if(!UserId.HasValue) return Unauthorized();
        var result = await deleteInvetory.ExecuteDeleteInventory(InventoryId, UserId.Value, cancellation);
        return result.Result();
    }

}