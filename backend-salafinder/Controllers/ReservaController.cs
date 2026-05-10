using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
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

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await _reserva_service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            var result = await _reserva_service.GetById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservaDTO_Create reserva) {
            var created = await _reserva_service.Create(
                reserva.fecha,
                reserva.hora_inicio,
                reserva.hora_fin,
                reserva.proposito,
                reserva.asistentes,
                reserva.id_espacio
            );

            return created != null ? CreatedAtAction(nameof(GetById),
                new { id = created.id }, reserva) : BadRequest(new { message = "Ocurrio un error." });
        }

        [HttpPut("{id}, {status}")]
        public async Task<IActionResult> ChangeStatus(Guid id, string status) {
            var status_changed = await _reserva_service.ChangeStatus(id, status);
            return status_changed == true ? Ok("Estado cambiado") : NotFound();
        }
    }
}
