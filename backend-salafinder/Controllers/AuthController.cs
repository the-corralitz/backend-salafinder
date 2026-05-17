using backend_salafinder.Interfaces;
using backend_salafinder.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_salafinder.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller {
        private readonly IAuthService _auth_service;

        public AuthController(IAuthService auth_service) {
            _auth_service = auth_service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto) {
            try {
                var response = await _auth_service.Register(dto);
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto) {
            try {
                var response = await _auth_service.Login(dto);
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        public IActionResult Index() {
            return View();
        }
    }
}
