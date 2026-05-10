using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
using backend_salafinder.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend_salafinder.Services {
    public class EspacioService : IEspacioService {
        private readonly ApplicationDbContext _context;
        public EspacioService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<Espacio>> GetAll() {
            return await _context.Espacio.ToListAsync();
        }

        public async Task<Espacio> GetById(Guid id) {
            return await _context.Espacio.FindAsync(id);
        }

        public async Task<Espacio> Create(
            string nombre,
            string tipo,
            int capacidad,
            string edificio,
            string descripcion,
            string[] recursos,
            string[] programas_prioritarios,
            bool requiere_aprobacion
        ) {
            var espacio = new Espacio {
                nombre = nombre,
                tipo = tipo,
                capacidad = capacidad,
                edificio = edificio,
                descripcion = descripcion,
                recursos = recursos,
                programas_prioritarios = programas_prioritarios,
                requiere_aprobacion = requiere_aprobacion
            };

            _context.Espacio.Add(espacio);
            await _context.SaveChangesAsync();
            return espacio;
        }

        public async Task<Espacio> Edit(Guid id, EspacioDTO espacio) {
            var existe = await _context.Espacio.FindAsync(id);
            if (existe == null) return null;

            if(espacio.nombre != null)
                existe.nombre = espacio.nombre;

            if (espacio.tipo != null)
                existe.tipo = espacio.tipo;

            if (espacio.capacidad != null)
                existe.capacidad = espacio.capacidad.Value;

            if (espacio.edificio != null)
                existe.edificio = espacio.edificio;

            if (espacio.descripcion != null)
                existe.descripcion = espacio.descripcion;

            if (espacio.recursos != null)
                existe.recursos = espacio.recursos;

            if (espacio.programas_prioritarios != null)
                existe.programas_prioritarios = espacio.programas_prioritarios;

            if (espacio.requiere_aprobacion != null)
                existe.requiere_aprobacion = espacio.requiere_aprobacion.Value;

            existe.ultima_vez_modificado = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await _context.Espacio.FindAsync(id);
        }
    }
}
