using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
using backend_salafinder.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend_salafinder.Services {
    public class UsuarioPerfilService : IUsuarioPerfilService {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public UsuarioPerfilService(ApplicationDbContext context, UserManager<IdentityUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<UsuarioPerfil>> GetAllUsers() {
            return await _context.UsuarioPerfil
                .Include(u => u.identity_user)
                .ToListAsync();
        }

        public async Task<UsuarioPerfil> GetUserById(Guid id) {
            return await _context.UsuarioPerfil
                .Include(u => u.identity_user)
                .FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task<bool> CambiarRol(CambiarRolDTO dto) {
            var roles_permitidos = new[] { "Student", "Staff" };
            if (!roles_permitidos.Contains(dto.nuevo_rol))
                throw new Exception("Sólo se asignar el rol 'Student' o Staff'.");

            var perfil = await _context.UsuarioPerfil
                .FirstOrDefaultAsync(u => u.id == dto.usuario_perfil_id);
            if (perfil == null)
                throw new Exception("Usuario no encontrado.");

            var identity_user = await _userManager.FindByIdAsync(perfil.identity_user_id);
            if (identity_user == null)
                throw new Exception("Usuario de autenticación no encontrado.");

            var roles_actuales = await _userManager.GetRolesAsync(identity_user);
            if (roles_actuales.Contains(dto.nuevo_rol))
                throw new Exception($"El usuario ya tiene el rol {dto.nuevo_rol}.");

            await _userManager.RemoveFromRolesAsync(identity_user, roles_actuales);
            await _userManager.AddToRoleAsync(identity_user, dto.nuevo_rol);

            return true;
        }

        public async Task<bool> RegistrarNoShow(RegistrarNoShowDTO dto) {
            var usuario = await _context.UsuarioPerfil.FirstOrDefaultAsync(u => u.id == dto.id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.no_shows++;

            if (usuario.no_shows >= 2) {
                usuario.bloqueado_hasta = DateTime.UtcNow.AddDays(7);
                usuario.no_shows = 0;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
