using System.Linq.Expressions;
using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
using backend_salafinder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_salafinder.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ReservaController : Controller {
        private readonly IReservaService _reserva_service;
        public ReservaController(IReservaService reserva_service) {
            _reserva_service = reserva_service;
        }
        public IActionResult Index() {
            return View();
        }

        private Guid GetUsuarioPerfilId()
        {
            var claim = User.FindFirst("usuario_perfil_id")?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() {
            if (User.IsInRole("Admin"))
                return Ok(await _reserva_service.GetAll());

            var usuario_id = GetUsuarioPerfilId();
            if (usuario_id == Guid.Empty)
                return Unauthorized(new { message = "Token inválido." });
            return Ok(await _reserva_service.GetByUsuario(usuario_id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            var result = await _reserva_service.GetById(id);

            if (result == null)
                return NotFound();

            if (!User.IsInRole("Admin")) {
                var usuarioId = GetUsuarioPerfilId();
                if (result.id_usuario != usuarioId)
                    return Forbid();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservaDTO_Create reserva) {
            var usuarioId = GetUsuarioPerfilId();
            if (usuarioId == Guid.Empty)
                return Unauthorized(new { message = "Token inválido." });

            try {
                var created = await _reserva_service.Create(
                    reserva.fecha,
                    reserva.hora_inicio,
                    reserva.hora_fin,
                    reserva.proposito,
                    reserva.asistentes,
                    reserva.id_espacio,
                    usuarioId
                );

                return CreatedAtAction(nameof(GetById), new { id = created.id }, reserva);
            } catch (Exception ex) { 
                return BadRequest(new { message = ex.Message }); 
            }
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> Cancelar([FromBody] CancelarDTO reserva) {
            var id_usuario = GetUsuarioPerfilId();
            if (id_usuario == Guid.Empty)
                return Unauthorized(new { message = "Token inválido" });

            try {
                var result = await _reserva_service.Cancelar(reserva.id, id_usuario);
                if (!result)
                    return NotFound(new { message = "No se encontró la reserva" });
                return Ok(new { message = "Se canceló la reserva exitosamente" });
            } catch (Exception e) {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] CambiarEstadoDTO estado) {
            try {
                var status_changed = await _reserva_service.ChangeStatus(id, estado.estado);
                if (!status_changed)
                    return NotFound(new { message = "Reserva no encontrada." });
                return Ok(new { message = $"Estado cambiado a {estado.estado}" });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
