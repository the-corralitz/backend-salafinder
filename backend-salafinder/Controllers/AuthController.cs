using backend_salafinder.Interfaces;
using backend_salafinder.Models.DTO;
using backend_salafinder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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

        // POST /auth/login
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

        [HttpPatch("cambiar-rol")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CambiarRol([FromBody] CambiarRolDTO dto) {
            try {
                await _auth_service.CambiarRol(dto);
                return Ok(new { message = $"Rol actualizado a {dto.nuevo_rol} correctamente." });
            } catch (Exception e) {
                return BadRequest(new { message = e.Message });
            }
        }

        public IActionResult Index() {
            return View();
        }
    }
}
