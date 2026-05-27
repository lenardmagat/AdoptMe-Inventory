using Microsoft.AspNetCore.Mvc;
using inventory.ErrorHandling;
namespace inventory.Helper;
public static class ControllerHelper
{
    public static IActionResult Result(this Result result)
    {
        if (!result.IsSuccess)
        {
            return new ObjectResult(new
            {
                error = result.Error,
                timestamp = DateTime.UtcNow
            }
            )
            {
                StatusCode = result.StatusCode
            };
        }
        return new OkObjectResult(result);
    } 
}