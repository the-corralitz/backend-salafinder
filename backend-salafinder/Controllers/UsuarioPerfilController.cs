using backend_salafinder.Interfaces;
using backend_salafinder.Models.DTO;
using backend_salafinder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend_salafinder.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UsuarioPerfilController : ControllerBase {
        private readonly IUsuarioPerfilService _service;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioPerfilController(
            IUsuarioPerfilService service,
            UserManager<IdentityUser> userManager) {
            _service = service;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() {
            var perfiles = await _service.GetAllUsers();
            var result = new List<UsuarioPerfilDTO>();

            foreach(var perfil in perfiles) {
                var roles = await _userManager.GetRolesAsync(perfil.identity_user!);
                var rol = roles.FirstOrDefault() ?? "Student";

                result.Add(new UsuarioPerfilDTO {
                    id = perfil.id,
                    nombre_completo = perfil.nombre_completo,
                    email = perfil.identity_user?.Email ?? string.Empty,
                    rol = rol,
                    no_shows = perfil.no_shows,
                    bloqueado_hasta = perfil.bloqueado_hasta,
                    esta_bloqueado = perfil.bloqueado_hasta.HasValue &&
                        DateTime.UtcNow < perfil.bloqueado_hasta.Value,
                });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id) {
            var perfil = await _service.GetUserById(id);
            if (perfil == null)
                return NotFound(new { message = "Usuario no encontrado" });

            var roles = await _userManager.GetRolesAsync(perfil.identity_user!);
            var rol = roles.FirstOrDefault() ?? "Student";

            return Ok(new UsuarioPerfilDTO {
                id = perfil.id,
                nombre_completo = perfil.nombre_completo,
                email = perfil.identity_user?.Email ?? string.Empty,
                rol = rol,
                no_shows = perfil.no_shows,
                bloqueado_hasta = perfil.bloqueado_hasta,
                esta_bloqueado = perfil.bloqueado_hasta.HasValue &&
                    DateTime.UtcNow < perfil.bloqueado_hasta.Value,
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("rol/cambiar")]
        public async Task<IActionResult> CambiarRol([FromBody] CambiarRolDTO dto) {
            try {
                await _service.CambiarRol(dto);
                return Ok(new { message = $"Rol actualizado a {dto.nuevo_rol} correctamente." });
            } catch (Exception e) {
                return BadRequest(new { message = e.Message });
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPatch("registrar_no_show")]
        public async Task<IActionResult> RegistrarNoShow([FromBody] RegistrarNoShowDTO dto) {
            try {
                await _service.RegistrarNoShow(dto);
                return Ok(new { message = $"No shows aumentados exitosamente" });
            } catch(Exception e) {
                return NotFound(new { message = e.Message });
            }
        }
    }
}
