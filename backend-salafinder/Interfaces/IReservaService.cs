using backend_salafinder.Models;

namespace backend_salafinder.Interfaces {
    public interface IReservaService {
        Task<List<Reserva>> GetAll();
        Task<Reserva> GetById(Guid id);
        Task<Reserva> Create(
            DateOnly fecha,
            TimeOnly hora_inicio,
            TimeOnly hora_fin,
            string proposito,
            int asistentes,
            Guid id_espacio
        );

        Task<bool> ChangeStatus(Guid id, string estado);
    }
}
