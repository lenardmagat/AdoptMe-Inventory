using Microsoft.AspNetCore.Mvc;
using inventory.Services;
namespace inventory.Controllers.Services;
public partial class ServiceController
{
    [HttpGet("PetLibrary")]
    public async Task<IActionResult> GetPetLibrary(
        [FromServices] IGetItemLibraryService getPetLibrary,
        CancellationToken cancellation
    )
    {
        var result =  await getPetLibrary.ExecuteGetItemLibrary(cancellation);
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