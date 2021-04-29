using Microsoft.AspNetCore.Mvc;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class WikiController : ControllerBase
    {
        [HttpGet]
        public IActionResult RedirectToWiki()
        {
            return Redirect("https://github.com/impact-moe/impact-api/wiki");
        }
    }
}