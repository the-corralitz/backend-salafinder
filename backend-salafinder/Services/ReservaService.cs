using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Reserva>> GetByUsuario(Guid id) {
            return await _context.Reserva
                .Include(r => r.espacio)
                .Include(r => r.usuario)
                .Where(r => r.id_usuario == id)
                .OrderByDescending(r => r.creado_en)
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
            if (espacio == null) 
                throw new Exception("El espacio no existe");

            var usuario = await _context.UsuarioPerfil.FindAsync(id_usuario);
            if (usuario == null) 
                throw new Exception("Usuario no encontrado");

            if (usuario.bloqueado_hasta.HasValue &&
            DateTime.UtcNow < usuario.bloqueado_hasta.Value)
            {
                var dias = (int)Math.Ceiling(
                    (usuario.bloqueado_hasta.Value - DateTime.UtcNow).TotalDays);
                throw new Exception(
                    $"Tu cuenta está bloqueada por {dias} día(s) por no-shows.");
            }

            var reservasActivas = await _context.Reserva
            .CountAsync(
                r => r.id_usuario == id_usuario &&
                (r.estado == "Pendiente" || r.estado == "Aprobado")
            );

            if (reservasActivas >= 3)
                throw new Exception(
                    "Tienes el máximo de reservas activas permitidas (3).");

            var fechaHoraReserva = fecha.ToDateTime(hora_inicio);
            if (fechaHoraReserva < DateTime.Now.AddHours(1))
                throw new Exception(
                    "Debes reservar con al menos 1 hora de anticipación.");

            if (hora_fin <= hora_inicio)
                throw new Exception(
                    "La hora de fin debe ser mayor a la hora de inicio.");

            if (asistentes > espacio.capacidad)
                throw new Exception(
                    $"El espacio tiene capacidad máxima de {espacio.capacidad} personas.");

            var conflicto = await _context.Reserva
            .FirstOrDefaultAsync(
                r =>
                r.id_espacio == id_espacio &&
                r.fecha == fecha &&
                //r.estado == "Aprobado" &&
                r.hora_inicio < hora_fin &&
                r.hora_fin > hora_inicio);

            if (conflicto != null)
                throw new Exception(
                    $"Conflicto de horario: el espacio ya está reservado " +
                    $"de {conflicto.hora_inicio} a {conflicto.hora_fin}.");


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
            var estadosValidos = new[] { "Pendiente", "Aprobado", "Cancelado" };
            if (!estadosValidos.Contains(estado))
                throw new Exception("Estado no válido.");

            var obj_aprove = await _context.Reserva.FindAsync(id);
            if (obj_aprove == null) return false;

            obj_aprove.estado = estado;
            obj_aprove.ultima_vez_modificado = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Cancelar(Guid id, Guid id_usuario) {
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null) return false;

            if (reserva.id_usuario != id_usuario)
                throw new Exception("No está autorizado para cancelar esta reserva.");

            if (reserva.estado == "Cancelado")
                throw new Exception("La reserva ya fue cancelada.");

            reserva.estado = "Cancelado";
            reserva.ultima_vez_modificado = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
