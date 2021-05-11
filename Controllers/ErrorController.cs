using Microsoft.AspNetCore.Mvc;

namespace ImpactApi.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => new StatusCodeResult(500); // Error logging already handled by ExceptionHandlerMiddleware.
    }
}
