using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
using Microsoft.Extensions.Primitives;

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
    }
}
