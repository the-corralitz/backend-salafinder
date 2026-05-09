using backend_salafinder.Models;

namespace backend_salafinder.Interfaces {
    public interface IEspacioService {
        Task<List<Espacio>> GetAll();
        Task<Espacio> GetById(Guid id);
        Task<Espacio> Create(Espacio espacio);
        Task<bool> Edit(Guid id, Espacio espacio);
        Task<int> ChangeStatus(Guid id);
    }
}
