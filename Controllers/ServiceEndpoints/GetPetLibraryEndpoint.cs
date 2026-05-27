using Microsoft.AspNetCore.Mvc;
using inventory.Services;
using inventory.Helper;
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
        return result.Result();
    }
}