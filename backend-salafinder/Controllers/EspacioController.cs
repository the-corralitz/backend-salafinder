using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_salafinder.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class EspacioController : Controller {
        private readonly IEspacioService _espacio_service;
        public EspacioController(IEspacioService espacio_service) {
            _espacio_service = espacio_service;
        }
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() {
            return Ok(await _espacio_service.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id) {
            var result = await _espacio_service.GetById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]EspacioDTO espacio) {
            var created = await _espacio_service.Create(
                espacio.nombre,
                espacio.tipo,
                espacio.capacidad ?? 0,
                espacio.edificio,
                espacio.descripcion,
                espacio.recursos,
                espacio.programas_prioritarios,
                espacio.requiere_aprobacion ?? false
            );

            return created != null ? CreatedAtAction(nameof(GetById),
                new { id = created.id }, espacio) : BadRequest(new { message = "IDs repetidos."});
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(DeleteEspacioDTO espacio) {
            try {
                var deleted = await _espacio_service.Delete(espacio.id);
                if (!deleted)
                    return NotFound("No se encontró ningún espacio");
                return Ok("Espacio eliminado exitosamente");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [FromBody]EspacioDTO espacio) {
            var edited = await _espacio_service.Edit(id, espacio);
            return edited != null ? Ok(edited) : NotFound(null);
        }
    }
}
