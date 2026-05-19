using backend_salafinder.Models;
using backend_salafinder.Models.DTO;

namespace backend_salafinder.Interfaces {
    public interface IUsuarioPerfilService {
        Task<List<UsuarioPerfil>> GetAllUsers();
        Task<UsuarioPerfil?> GetUserById(Guid id);
        Task<bool> CambiarRol(CambiarRolDTO rolDTO);
        Task<bool> RegistrarNoShow(RegistrarNoShowDTO id);
        Task<List<UsuarioPerfil>> GetOnlyStudents();
    }
}
