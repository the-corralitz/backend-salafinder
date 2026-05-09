using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

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
            bool requiere_aprobacion) {
            var espacio = new Espacio { nombre = nombre,
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

        public async Task<Espacio> Edit(Guid id, Espacio espacio) {
            var existe = await _context.Espacio.FindAsync(id);
            if (existe == null) return null;

            existe.nombre = espacio.nombre;
            existe.tipo = espacio.tipo;
            existe.edificio = espacio.edificio;
            existe.descripcion = espacio.descripcion;
            existe.recursos = espacio.recursos;
            existe.programas_prioritarios = espacio.programas_prioritarios;

            await _context.SaveChangesAsync();
            return existe;
        }
    }
}
