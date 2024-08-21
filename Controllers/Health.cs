using Microsoft.AspNetCore.Mvc;

namespace windows_audio_api.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "Healthy" });
        }
    }
}