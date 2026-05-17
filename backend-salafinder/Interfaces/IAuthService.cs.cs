using backend_salafinder.Models;
using backend_salafinder.Models.DTO;

namespace backend_salafinder.Interfaces {
    public interface IAuthService {
        Task<AuthResponseDTO> Register(RegisterDTO registerDTO);
        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
    }
}
