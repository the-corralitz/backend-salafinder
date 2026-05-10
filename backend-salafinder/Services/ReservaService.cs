using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace backend_salafinder.Services {
    public class ReservaService : IReservaService {
        private readonly ApplicationDbContext _context;
        public ReservaService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<Reserva>> GetAll() {
            return await _context.Reserva
                .Include(r => r.espacio)
                .ToListAsync();
        }

        public async Task<Reserva> GetById(Guid id) {
            return await _context.Reserva
                .Include(r => r.espacio)
                .FirstOrDefaultAsync(r => r.id == id);
        }

        public async Task<Reserva> Create(
            DateOnly fecha,
            TimeOnly hora_inicio,
            TimeOnly hora_fin,
            string proposito,
            int asistentes,
            Guid id_espacio
        ) {
            var espacio = await _context.Espacio.FindAsync(id_espacio);
            if (espacio == null) return null;

            var reserva = new Reserva {
                fecha = fecha,
                hora_inicio = hora_inicio,
                hora_fin = hora_fin,
                proposito = proposito,
                asistentes = asistentes,
                id_espacio = id_espacio,
                espacio = espacio
            };

            reserva.espacio = espacio;
        
            _context.Reserva.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<bool> ChangeStatus(Guid id, string estado) {
            var obj_aprove = await _context.Reserva.FindAsync(id);
            if (obj_aprove == null) return false;

            obj_aprove.estado = estado;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
