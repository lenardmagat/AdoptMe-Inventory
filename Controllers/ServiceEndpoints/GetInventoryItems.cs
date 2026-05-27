using System.Security.Cryptography.X509Certificates;
using inventory.Helper;
using inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpGet("Inventory/{InventoryId}")]
    public async Task<IActionResult> GetInvetoryItems(
        [FromServices] IGetUserInvetoryItemsService getUserInvetoryItems,
        CancellationToken cancellation,
        string InventoryId
    )
    {
        var result = await getUserInvetoryItems.ExecuteGetUserInventoryItems(InventoryId, cancellation);
        return result.Result();
    }
}