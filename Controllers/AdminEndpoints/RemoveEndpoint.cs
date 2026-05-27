using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
using inventory.Helper;
namespace inventory.Controllers.AdminControllers;
public partial class AdminController
{
    [HttpDelete("Remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveItems(
        [FromBody] ItemHashids itemHashids,
        [FromServices] IRemoveItems removeItems,
        CancellationToken cancellationToken
    )
    {
        var result = await removeItems.RemoveExecute(itemHashids, cancellationToken);
        return result.Result();
    }
}