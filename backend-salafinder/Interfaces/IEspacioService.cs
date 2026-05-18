using backend_salafinder.Models;
using backend_salafinder.Models.DTO;

namespace backend_salafinder.Interfaces {
    public interface IEspacioService {
        Task<List<Espacio>> GetAll();
        Task<Espacio> GetById(Guid id);
        Task<Espacio> Create(
            string nombre,
            string tipo,
            int capacidad,
            string edificio,
            string descripcion,
            string[] recursos,
            string[] programas_prioritarios,
            bool requiere_aprobacion
        );
        Task<Espacio> Edit(Guid id, EspacioDTO espacio);
        Task<bool> Delete(Guid id);
    }
}
