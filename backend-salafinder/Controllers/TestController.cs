using Microsoft.AspNetCore.Mvc;

namespace backend_salafinder.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller {
        [HttpGet]
        public IActionResult Get() {
            return Ok(new { message = "SalaFinder API funcionando ✅", version = "1.0" });
        }
        public IActionResult Index() {
            return View();
        }
    }
}
