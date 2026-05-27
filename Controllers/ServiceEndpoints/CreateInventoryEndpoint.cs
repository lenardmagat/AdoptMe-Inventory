using Microsoft.AspNetCore.Mvc;
using inventory.Services;
using inventory.DTOs;
using Microsoft.AspNetCore.Authorization;
using inventory.Extensions;
using inventory.Helper;
namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpPost("Create")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> CreateInventory(
        [FromBody] CreateInventoryDetails inventoryDetails,
        [FromServices] ICreateInventoryService createInventory,
        CancellationToken cancellation
    )
    {
        var UserId = User.GetUserId();
        if(!UserId.HasValue || UserId == 0 || UserId.Value == 0) return Unauthorized();
        var result = await createInventory.ExecuteCreateInVentory(UserId.Value, inventoryDetails, cancellation);
        return result.Result();
    }
}