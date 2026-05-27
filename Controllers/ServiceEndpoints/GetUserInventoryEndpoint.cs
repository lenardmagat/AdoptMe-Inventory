using inventory.Extensions;
using inventory.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpGet("UserInvetories")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetUserInventories(
        [FromServices] IGetUserInventoriesService getUserInventories,
        CancellationToken cancellation
        )
    {
        var UserId = User.GetUserId();
        if(!UserId.HasValue) return Unauthorized();
        var result = await getUserInventories.ExecuteGetUserInventories(UserId.Value, cancellation);
        if(!result.IsSuccess) 
            return  StatusCode(
                result.StatusCode, new
                {
                    error = result.Error,
                    timestampt = DateTime.UtcNow
                }
            );
        return Ok(result);
    }
}