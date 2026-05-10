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
                .Include(r => r.usuario)
                .ToListAsync();
        }

        public async Task<Reserva> GetById(Guid id) {
            return await _context.Reserva
                .Include(r => r.espacio)
                .Include(r => r.usuario)
                .FirstOrDefaultAsync(r => r.id == id);
        }

        public async Task<Reserva> Create(
            DateOnly fecha,
            TimeOnly hora_inicio,
            TimeOnly hora_fin,
            string proposito,
            int asistentes,
            Guid id_espacio,
            Guid id_usuario
        ) {
            var espacio = await _context.Espacio.FindAsync(id_espacio);
            if (espacio == null) return null;

            var usuario = await _context.UsuarioPerfil.FindAsync(id_usuario);
            if (usuario == null) return null;

            var reserva = new Reserva {
                fecha = fecha,
                hora_inicio = hora_inicio,
                hora_fin = hora_fin,
                proposito = proposito,
                asistentes = asistentes,
                id_espacio = id_espacio,
                espacio = espacio,
                usuario = usuario
            };
        
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
