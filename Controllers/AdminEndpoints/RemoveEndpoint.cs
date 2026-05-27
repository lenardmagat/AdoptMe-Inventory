using Microsoft.AspNetCore.Mvc;
using inventory.DTOs;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
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
        if(!result.IsSuccess)
            return StatusCode(
                result.StatusCode, new
                {
                    error = result.Error,
                    timestampt = DateTime.UtcNow
                }
            );
        return Ok(result);
    }
}